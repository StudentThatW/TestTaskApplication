using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TestTaskApplication.Services;
using TestTaskApplication.VkService;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace TestTaskApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticController : Controller
    {

        private readonly LetterStatisticContext _context;

        private readonly ILogger<StatisticController> _logger;

        private VKService vkService;

        private FrequencyService frequencyService;

        public StatisticController(ILogger<StatisticController> logger, LetterStatisticContext context)
        {
            _logger = logger;
            _context = context;
            vkService = new VKService();
            frequencyService = new FrequencyService();
        }

        private void SaveResults(int vk_id, List<LetterFrequency> frequencyList)
        {
            if (!_context.Users.Any(u => u.UserId == vk_id))
            {
                var newUser = new User { UserId = vk_id };
                _context.Users.AddRange(newUser);
                _context.SaveChanges();
            }
            var user = _context.Users.FirstOrDefault(u => u.UserId == vk_id);

            foreach (var letterFrequency in frequencyList)
            {
                letterFrequency.Owner = user;
            }

            _context.LetterFrequencies.RemoveRange(_context.LetterFrequencies.Where(u => u.OwnerId == vk_id));
            _context.LetterFrequencies.AddRange(frequencyList);
            _context.SaveChanges();

        }

        [HttpPost("/vkuser/general-post-statistic")]
        public ActionResult<Dictionary<String, int>> PostVKGeneral(int vk_id)
        {
            _logger.LogInformation("Start count: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff"));
            var response = vkService.GetVKResponse(vk_id);
            if (response.IsError)
                return NotFound(response.ErrorMessage);
            var frequencyList = frequencyService.GetLettersFrequency(response.MessageDict);
            _logger.LogInformation("End count: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff"));
            var generalStatistic = frequencyService.GetLettersFrequencyGeneral(frequencyList);
            SaveResults(vk_id, frequencyList);
            return generalStatistic;
        }

        [HttpPost("/vkuser/separate-posts-statistic")]
        public ActionResult<Dictionary<int, Dictionary<String, int>>> PostVK(int vk_id)
        {
            _logger.LogInformation("Start count: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff"));
            var response = vkService.GetVKResponse(vk_id);
            if (response.IsError)
                return NotFound(response.ErrorMessage);
            var frequencyList = frequencyService.GetLettersFrequency(response.MessageDict);
            _logger.LogInformation("End count: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff"));
            var separateStatistic = frequencyService.GetLettersFrequency(frequencyList);
            SaveResults(vk_id, frequencyList);
            return separateStatistic;
        }
    }
}
