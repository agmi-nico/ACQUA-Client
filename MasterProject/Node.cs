﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterProject
{
    public enum EType
    {
        UD, DD, UBW, DBW, ULR, DLR, LEAF
    }

    class Node
    {
        public static int[] Values;
        public Node Right;
        public Node Left;
        public EType VAR;
        public string OP;
        public string VAL;

        public Node(string inputVAR, string OP, string VAL)
        {
            this.VAR = GetVal(inputVAR);
            this.OP = OP;
            this.VAL = VAL;
        }

        public EType GetVal(string inputVAR)
        {
            switch (inputVAR.ToUpper())
            {
                case "UD":
                    return EType.UD;
                case "DD":
                    return EType.DD;
                case "UBW":
                    return EType.UBW;
                case "DBW":
                    return EType.DBW;
                case "LR": //LR transform to ULR because currently there is no difference between ULR and DLR, and i didnt want to change the xml files xD
                    return EType.ULR;
                case "ULR":
                    return EType.ULR;
                case "DLR":
                    return EType.DLR;
                case "LEAF":
                    return EType.LEAF;
                default:
                    throw new Exception("ERORR");
            }
        }

        public bool isLeaf()
        {
            return EType.LEAF.Equals(VAR);
        }

        public int evalLeaf()
        {
            if(!this.isLeaf())
            {
                throw new Exception("can't evalLeaf(): node is not a leaf");
            }
            switch (OP)
            {
                case "NOCALL":
                    return 0;
                case "BAD":
                    return 1;
                case "GOOD":
                    return 2;
                case "EXCELLENT":
                    return 3;
                default:
                    return -1;
            }
        }

        public bool IsRight()
        {
            int intVAL;
            if (int.TryParse(VAL, out intVAL))
            {
                switch (OP)
                {
                    case ">":
                        return Values[(int)VAR] > intVAL;
                    case ">=":
                        return Values[(int)VAR] >= intVAL;
                    case "<":
                        return Values[(int)VAR] < intVAL;
                    case "<=":
                        return Values[(int)VAR] <= intVAL;
                    default:
                        throw new Exception("ERROR evaluating expression");
                }
            }
            throw new Exception("ERROR evaluating expression");
        }
    }
}
