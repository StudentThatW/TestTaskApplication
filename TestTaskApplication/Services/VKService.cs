using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace TestTaskApplication.VkService
{
    public class Response
    {
        public bool IsError;

        public string ErrorMessage;

        public Dictionary<int, String> MessageDict;
    }

    public class VKService
    {

        private VkApi _api;
        public VKService()
        {
            _api = new VkApi();
            _api.Authorize(new ApiAuthParams
            {
                AccessToken = "{your_access_token}"                
            });
        }

        public Response GetVKResponse(int vk_id)
        {
            try
            {
                var get = _api.Wall.Get(new WallGetParams
                {
                    OwnerId = vk_id,
                    Count = 5,
                });
                var messageDict = GetMessagesText(get);
                return new Response { MessageDict = messageDict};
            }
            catch (Exception e)
            {
                return new Response { IsError = true, ErrorMessage = e.Message };
            }           
        }

        public Dictionary<int, String> GetMessagesText(WallGetObject get)
        {            
            var messageDict = new Dictionary<int, String>();
            var counter = 0;
            foreach (Post post in get.WallPosts)
            {
                var resultText = "";
                if (post.Text != "")
                    resultText += post.Text;
                if (post.CopyHistory != null)
                {
                    foreach (var repost in post.CopyHistory)
                    {
                        resultText += repost.Text;
                    }
                }
                messageDict[counter] = resultText;
                counter += 1;
            }
            
            return messageDict;
        }
    }
}
