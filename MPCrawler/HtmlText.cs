using System.Net.Http;
using System.Collections.Generic;
using System;

namespace MpCrawler
{
    // this file includes:
    // 1) HtmlText Class declariation
    // 2) HtmlFind Class:  functions to find tag/keywords

    public class HtmlText
    {
        // var: url, url text
        // function: find tag with key words in html content
        // todo : shall we define search result as the variable in class? pro vs cons
        // todo : how to know if intermediate search result list is cleaned

        public string url;
        public string htmlText;

        public HtmlText(string url)
        {
            // Function:
            //          get html text
            // Todo:
            //          how do we determine is url is responding and content is valid

            this.url = url;
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            htmlText = html.Result;
        }


        public HtmlText()
        {
        }
    }



    public class HtmlFind
    {
        // List<string> HtmlFindAllTags(string htmlText, string tagName)
        // List<string> HtmlFindAllTags(string htmlText, string tagName, string className)
        // string GetHyperLinks(string currStr, string linkName)

        public static List<string> HtmlFindAllTags(string htmlText, string tagName)
        {
            // Function:
            //              this function is to search any tag with tagName only
            //              find1 : <tagName> ... </tagName>
            
            return SearchByTagName(htmlText, tagName);
        }


        public static List<string> HtmlFindAllTags(string htmlText, string tagName, string typeName)
        {
            // Function:
            //              this function is to search any tag with tagName. Also, it shall include typeName 
            //              find2 : <tagName class="..."> ... </tagName>, <tagName href="..."> ... </tagName>
            // Notes:
            //              type can be " any="any... "

            return FilterByClassName(htmlText, SearchByTagName(htmlText, tagName), typeName);
        }


        public static string GetHyperLinks(string currStr, string linkName)
        {
            // Function:
            //              Specify the first <> as the search range, in case of nested tag
            //              Return the first found url begins with linkName. Otherwise empty string
            //              For instance, if "href="http://" is found within the first <>, extract link and add it to the list
            // Inputs:
            //              linkName: linkName can be "href="", "src="", the keywords before the first char of the url
            //
            // Todo:
            //              how to handle if more than one linkName is found?

            string hyperLink = "";             
            int searchEnd = currStr.IndexOf('>');
            string searchStr = currStr.Substring(0, searchEnd + 1);
            if (searchStr.Contains(linkName))
            {
                int linkBeginAt = currStr.IndexOf("http"); 
                int linkEndAt = currStr[linkBeginAt..searchEnd].IndexOf('"');  // find " which ends the url
                hyperLink = currStr.Substring(linkBeginAt, linkEndAt);
            }

            return hyperLink;
        }


        //public static List<string> HtmlFindAllTags(string htmlText, string tagName, string className, string linkName)
        //{
        //    // Function:
        //    //            to search any tag with tagName and className
        //    //            find3:  <tagName class = "className", href="http://..."> ... </tagName>
        //    //
        //    // Notes:
        //    //          only linktype as "herf=" is supported

        //    List<string> validTagFilterByTagName = SearchByTagName(htmlText, tagName);
        //    List<string> validTagFilterByClassName = FilterByClassName(htmlText, validTagFilterByTagName, className);
        //    return GetHyperLinks(htmlText, validTagFilterByClassName, linkName);
        //}

        private static List<string> SearchByTagName(string htmlText, string tagName)
        {
            // Function:
            //           looking for : < tagName > ... </ tagName >
            //           Handle the condition that within a qualified tag there are more than one nested qualified tags
            // 
            // Todo:
            //            handle fault when stack is not empty in the end, or end find while stack is empty

            List<string> validTagContent = new List<string>();
            Stack<int> nestedBeginTags = new Stack<int>();
            string tagBegin = "<" + tagName;         // "< tagName" 
            string tagEnd = "</" + tagName + ">";    // "</tagName>"
            
            for (int i = 0; i < htmlText.Length - tagBegin.Length; i++)
            {
                // skip if tagBegin is not found
                if (!htmlText.Substring(i, tagBegin.Length).Equals(tagBegin))
                    continue;
                
                for (int j = i; j < htmlText.Length - tagEnd.Length; j++)
                {
                    // if tag begin is found, push the index of first character into stack
                    if (htmlText.Substring(j, tagBegin.Length).Equals(tagBegin))
                    {
                        nestedBeginTags.Push(j);
                        j += tagBegin.Length;
                        continue;
                    }

                    // if end tag is found, pair with the most recent tag begin index
                    if (htmlText.Substring(j, tagEnd.Length).Equals(tagEnd))
                    {
                        int startIndex = nestedBeginTags.Pop();
                        int sectionLen = j + tagEnd.Length - startIndex;
                        validTagContent.Add(htmlText.Substring(startIndex, sectionLen));

                        // exit if all tag ends are found
                        if (nestedBeginTags.Count == 0)
                        {
                            i = j + tagEnd.Length;
                            break;
                        }
                    }
                }
             }
      
            return validTagContent;
        }

        private static List<string> FilterByClassName(string htmlText, List<string> tagContentList, string className)
        {
            // Function:
            //              look for < tagName ... class = ""... >
            //              check the string in the first <> in case of nested tags
            
            List<string> filterResult = new List<string>();
            
            for (int i = 0; i < tagContentList.Count; i++)
            {
                string currStr = tagContentList[i];
                int searchLen = currStr.IndexOf('>') + 1;
                
                if (currStr.Substring(0, searchLen).Contains(className))
                {
                    filterResult.Add(currStr);
                }
                    
            }

            return filterResult;
        }
    }
}
