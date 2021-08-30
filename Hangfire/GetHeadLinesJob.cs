using Hangfire;
using LeftRightNet.Data;
using LeftRightNet.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LeftRightNet.Hangfire
{
    public class HeadLineDto
    {
        public string photoIdName { get; set; }
        public List<string> headLines { get; set; }
    }
    public class GetHeadLinesJob
    {
        private readonly ApplicationDbContext _ctx;
        private readonly string applicationJson = "application/json";
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<GetHeadLinesJob> _logger;

        public GetHeadLinesJob(IHttpClientFactory clientFactory, ApplicationDbContext ctx, ILogger<GetHeadLinesJob> logger)
        {
            _ctx = ctx;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task Run(IJobCancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await RunAtTimeOf(DateTime.Now);
        }

        public async Task RunAtTimeOf(DateTime now)
        {
            var curConfig = _ctx.Configs.FirstOrDefault();
            if(curConfig == null)
            {
                return;
            }

            var allSites = _ctx.NewsSites.ToList();
            foreach (var site in allSites)
            {

                HttpClient client = _clientFactory.CreateClient("GetHeadLines");
                client.Timeout = TimeSpan.FromMinutes(5);
                string json = JsonConvert.SerializeObject(new
                {
                    SiteName = site.Name,
                    SiteUrl = site.Url
                });

                StringContent data = new StringContent(json, Encoding.UTF8, applicationJson);
                HttpResponseMessage response = null;
                try
                {
                    response = await client.PostAsync(curConfig.URLForScrapping, data);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Following Error occured Message {ex.Message}");
                    _logger.LogError($"Following Error occured StackTrace {ex.StackTrace}");
                    _logger.LogError($"Following Error occured InnerException {ex.InnerException}");

                }

                string result = response.Content.ReadAsStringAsync().Result;

                try
                {
                    var listOfHeadLines = JsonConvert.DeserializeObject<HeadLineDto>(result);

                    var newSnapShot = new SnapShot()
                    {
                        NewsSiteId = site.Id,
                        ImageHashId = listOfHeadLines.photoIdName,
                        CreatedAt = DateTime.UtcNow,
                    };

                    _ctx.SnapShots.Add(newSnapShot);

                    await _ctx.SaveChangesAsync();

                    foreach (var item in listOfHeadLines.headLines)
                    {

                        string jsonSentiment = JsonConvert.SerializeObject(new
                        {
                            data = item
                        });

                        StringContent dataJsonSentiment = new StringContent(jsonSentiment, Encoding.UTF8, applicationJson);
                        HttpResponseMessage responseSentiment = null;
                        try
                        {
                            responseSentiment = await client.PostAsync(curConfig.URLForSentiment, dataJsonSentiment);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Following Error occured Message {ex.Message}");
                            _logger.LogError($"Following Error occured StackTrace {ex.StackTrace}");
                            _logger.LogError($"Following Error occured InnerException {ex.InnerException}");

                        }

                        string resultSentiment = responseSentiment.Content.ReadAsStringAsync().Result;
                        var newHeadLine = new HeadLine()
                        {
                            ValueText = item,
                            CreatedAt = DateTime.UtcNow,
                            SnapShotId = newSnapShot.Id
                        };
                        _ctx.HeadLines.Add(newHeadLine);

                        await _ctx.SaveChangesAsync();

                        var newSentiment = JsonConvert.DeserializeObject<Sentiment>(resultSentiment);
                        newSentiment.HeadLineId = newHeadLine.Id;
                        _ctx.Sentiments.Add(newSentiment);

                        await _ctx.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Following DeserializeObject Error occured content of result {result}");
                    _logger.LogError($"Following DeserializeObject Error occured Message {ex.Message}");
                    _logger.LogError($"Following DeserializeObject Error occured StackTrace {ex.StackTrace}");
                    _logger.LogError($"Following DeserializeObject Error occured InnerException {ex.InnerException}");

                }
            }
           
        }
    }
}
