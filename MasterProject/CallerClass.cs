using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MasterProject
{
    class CallerClass
    {
        public static int Call(Node root, int D, int JTR, int UBW, int DBW, int DLR, int ULR)
        {
            Node.Values = new int[6];
            Node.Values[0] = D;
            Node.Values[1] = JTR;
            Node.Values[2] = UBW;
            Node.Values[3] = DBW;
            Node.Values[4] = ULR;
            Node.Values[5] = DLR;
            return Calculate(root);
        }

        public static Node Load(string path)
        {
            try
           {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                Node root;
                XmlElement element = doc.DocumentElement;
                recall(out root, element);
                return root;
            }
            catch
            {
                return null;
            }
        }

        public static int Calculate(Node root)
        {
            if (root == null)
                return -1;
            if (root.isLeaf())
            {
                System.Console.WriteLine("LEAF " + root.evalLeaf());
                return root.evalLeaf();
            }
            if (root.IsRight())
            {
                System.Console.WriteLine(root.VAR + " " + root.OP + " " + root.VAL + " =true");
                return Calculate(root.Right);
            }
            else
            {
                System.Console.WriteLine(root.VAR + " " + root.OP + " " + root.VAL + " =false");
                return Calculate(root.Left);
            }
        }

        public static void recall(out Node tem, XmlNode x)
        {
            if (x.ChildNodes.Count == 5)
            {
                tem = new Node(x.ChildNodes[0].InnerText, x.ChildNodes[1].InnerText, x.ChildNodes[2].InnerText);
                recall(out tem.Left, x.ChildNodes[3]);
                recall(out tem.Right, x.ChildNodes[4]);
            }
            else
            {
                if (x.ChildNodes.Count == 1)
                {
                    tem = new Node("LEAF", x.ChildNodes[0].InnerText, "");
                }
                else
                { 
                    tem = null;
                }
            }
        }
    }
}