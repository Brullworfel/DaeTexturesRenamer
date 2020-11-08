using System;
using System.Xml;
using System.Xml.XPath;
using System.Collections.Specialized;
using System.IO;
using System.Collections.Generic;

namespace DaeTexturesRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] dirs = { "AreaModelsMaterial", "AreaModelsSpectral", "ObjectModels" };
            foreach (string dir in dirs)
            {
                //Console.WriteLine("dir " + dir + ":");
                string[] files = Directory.GetFiles(@dir, "*.dae");
                int fileI = 0;
                foreach (string xmlFileName in files)
                {

                    fileI++;

                    Console.WriteLine(dir + " " + fileI + " / " + files.Length);
                    //continue;

                    NameValueCollection rewrites = GetRewritesList(xmlFileName);

                    if (rewrites.Count > 0)
                    {
                        ReplaceAllInFile(xmlFileName, rewrites);
                    }
                    else
                    {
                        //Console.WriteLine(xmlFileName + " not needs in rewrites");
                    }

                    RemoveDuplicateNodes(xmlFileName);
                }

            }
            Console.WriteLine("Done! Press any key to exit.");
            Console.ReadKey();
        }

        static NameValueCollection GetRewritesList(string xmlFileName)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlFileName);
            XPathNavigator navigator = document.CreateNavigator();

            XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
            manager.AddNamespace("collada", "http://www.collada.org/2005/11/COLLADASchema");

            NameValueCollection rewrites = new NameValueCollection();

            foreach (XPathNavigator image in navigator.Select("//collada:image", manager))
            {
                string imageId = image.GetAttribute("id", "");
                image.MoveToFirstChild();
                string imageFileName = image.Value;
                string oldName = imageId.Substring(0, imageId.IndexOf("-"));
                string newName = imageFileName.Substring(0, imageFileName.LastIndexOf("."));

                if (oldName == newName)
                {
                    continue;
                }

                rewrites[oldName] = newName;

            }

            return rewrites;
        }

        static void RemoveDuplicateNodes(string xmlFileName)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlFileName);
            XPathNavigator navigator = document.CreateNavigator();

            XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
            manager.AddNamespace("collada", "http://www.collada.org/2005/11/COLLADASchema");

            string[] searchNodes = { "image", "effect", "material"};

            foreach(string searchNode in searchNodes)
            {
                List<string> nodeIds = new List<string>();

                foreach (XPathNavigator effect in navigator.Select("//collada:" + searchNode, manager))
                {
                    string effectId = effect.GetAttribute("id", "");
                    if (nodeIds.Contains(effectId))
                        continue;
                    nodeIds.Add(effectId);
                }

                foreach (string nodeId in nodeIds)
                {

                    XPathNodeIterator nodes = navigator.Select("//collada:"+ searchNode + "[@id='" + nodeId + "']", manager);
                    if (nodes.Count < 2)
                        continue;

                    //Console.WriteLine(searchNode + " " + nodeId + " has duplicates");

                    int i = 0;
                    foreach (XPathNavigator node in nodes)
                    {
                        i++;
                        if (i == 1)
                            continue;
                        node.DeleteSelf();
                    }

                }
            }

            

            document.Save(xmlFileName);
        }

        static void ReplaceAllInFile(string fileName, NameValueCollection rewrites)
        {
            string text = File.ReadAllText(fileName);
            foreach (string key in rewrites)
            {
                text = text.Replace(key, rewrites[key]);
            }
            File.WriteAllText(fileName, text);
            //File.WriteAllText(fileName, text);
        }
    }

}
