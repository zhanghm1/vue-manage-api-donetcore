using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VueManage.Infrastructure.Common
{
    /// <summary>
    /// 语言管理器
    /// 注入时应为当前域单例
    /// </summary>
    public class LanguageManager
    {
        private readonly IMemoryCache _memoryCache;
        public LanguageManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            Console.WriteLine("构造LanguageManager");
            
        }
        public LanguageModel languageModel
        {
            get
            {
                return languageJObject.ToObject<LanguageModel>();
            }
        }
        public JObject languageJObject;

        private string languageCacheName = "language_";


        private string _Area { get; set; }
        /// <summary>
        /// 当前请求的地区
        /// 初始化对象之后请赋值以初始化对应的语言对象
        /// </summary>
        public string Area
        {
            get
            {
                return _Area;
            }
            set
            {
                _Area = value;
                if (!string.IsNullOrEmpty(value))
                {
                    Init(value);
                }
            }
        }

        public void Init(string area)
        {
            string cacheKey = languageCacheName + area;
            languageJObject = (JObject)_memoryCache.Get(cacheKey);
            if (languageJObject==null)
            {
                try
                {
                    string basePath = AppDomain.CurrentDomain.BaseDirectory;
                    // TODO 这里可以做成缓存，先读取缓存
                    var json = File.ReadAllText(basePath + "LanguageResources/" + area + ".json", Encoding.UTF8);
                    Console.WriteLine(json);
                    languageJObject = (JObject)JsonConvert.DeserializeObject(json);
                    _memoryCache.Set(cacheKey, languageJObject);
                }
                catch (Exception ex)
                {
                    // 日志
                }
            }
            
            
        }
        public string GetMessage(string key)
        {
            if (languageJObject==null)
            {
                return key;
            }
            return languageJObject[key].ToString();
        }
    }

    public class LanguageModel
    {
        public string SERVERFAIL { get; set; }
        public string SUCCESS { get; set; }
        public string FAIL { get; set; }
        public string Unauthorized { get; set; }
        public string NotFind { get; set; }
    }
}
