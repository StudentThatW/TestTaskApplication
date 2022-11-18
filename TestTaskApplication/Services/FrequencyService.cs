using System;
using System.Collections.Generic;
using System.Linq;

namespace TestTaskApplication.Services
{
    public class FrequencyService
    {
        public List<LetterFrequency> GetLettersFrequency(Dictionary<int, String> messageDict)
        {
            var frequencyList = new List<LetterFrequency>();
            foreach (var messageNumber in messageDict.Keys)
            {
                var messageText = messageDict[messageNumber];
                messageText
                    .Where(char.IsLetter)
                    .GroupBy(char.ToLower)
                    .OrderBy(g => g.Key)
                    .ToList()
                    .ForEach(g => frequencyList.Add(new LetterFrequency
                    {
                        Frequency = g.Count(),
                        Letter = g.Key.ToString(),
                        MessageNumber = messageNumber
                    }));
            }
            return frequencyList;
        }

        public Dictionary<int, Dictionary<String, int>> GetLettersFrequency(List<LetterFrequency> frequencyList)
        {           
            var frequencyDict = new Dictionary<int, Dictionary<String, int>>();
            for (int i = 0; i < 5; i++)
            {
                frequencyDict[i] = new Dictionary<string, int>();
                frequencyList
                    .Where(g => g.MessageNumber == i)
                    .OrderBy(g => g.Letter)
                    .ToList()
                    .ForEach(g => frequencyDict[i][g.Letter] = (int)g.Frequency);
            }
            return frequencyDict;
        }

        public Dictionary<String, int> GetLettersFrequencyGeneral(List<LetterFrequency> frequencyList)
        {            
            var frequencyDict = GetLettersFrequency(frequencyList);
            var generalFrequencyDict = new Dictionary<String, int>();
            foreach (var messageNumber in frequencyDict.Keys)
            {
                foreach (var letter in frequencyDict[messageNumber].Keys)
                {
                    if (!generalFrequencyDict.ContainsKey(letter))
                        generalFrequencyDict[letter] = frequencyDict[messageNumber][letter];
                    else
                        generalFrequencyDict[letter] += frequencyDict[messageNumber][letter];
                }
            }
            generalFrequencyDict = generalFrequencyDict.OrderBy(g => g.Key).ToDictionary(obj => obj.Key, obj => obj.Value); ;
            return generalFrequencyDict;
        }
    }
}
