using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Helper
{
    public class StringDestructure
    {
        private class PartialStringIndex
        {
            public string Context;
            public int RecomposeOrder;
            public int StartIndex;
            public int EndIndex;
        }

        List<PartialStringIndex> ElementsMeetCondition;
        List<PartialStringIndex> ElementsDoNotMeetCondition;
        List<Func<string, int, int, string>> ActionsOnConditionMet;
        List<Func<string, int, int, string>> ActionsOnConditionNotMet;

        private StringDestructure(string Context, Predicate<char> predicate)
        {
            ElementsMeetCondition = new List<PartialStringIndex>();
            ElementsDoNotMeetCondition = new List<PartialStringIndex>();
            ActionsOnConditionMet = new List<Func<string, int, int, string>>();
            ActionsOnConditionNotMet = new List<Func<string, int, int, string>>();

            StringBuilder temporary = new StringBuilder();

            bool storeToCondition = false;
            //bool storeToNoMeetCondition = false;
            int recomposeIndex = 0;
            int startIndex = 0;

            for (int i = 0; i < Context.Length; i++)
            {
                char ch = Context[i];
                if (i == 0)
                {
                    if (predicate(ch))
                    {
                        temporary.Append(ch);
                        storeToCondition = true;
                    }
                    else
                    {
                        temporary.Append(ch);
                        storeToCondition = false;
                    }
                }
                else
                {
                    if (predicate(ch))
                    {
                        if (storeToCondition)
                        {
                            temporary.Append(ch);
                        }
                        else
                        {
                            ElementsDoNotMeetCondition.Add(new PartialStringIndex
                            {
                                Context = temporary.ToString(),
                                RecomposeOrder = recomposeIndex,
                                StartIndex = startIndex,
                                EndIndex = i - 1
                            });
                            startIndex = i;
                            recomposeIndex++;
                            temporary.Clear();
                            temporary.Append(ch);
                        }
                        storeToCondition = true;
                    }
                    else//if (!predicate(ch)
                    {
                        if (storeToCondition)
                        {
                            ElementsMeetCondition.Add(new PartialStringIndex
                            {
                                Context = temporary.ToString(),
                                RecomposeOrder = recomposeIndex,
                                StartIndex = startIndex,
                                EndIndex = i - 1
                            });
                            startIndex = i;
                            recomposeIndex++;
                            temporary.Clear();
                            temporary.Append(ch);
                        }
                        else
                        {
                            temporary.Append(ch);
                        }
                        storeToCondition = false;
                    }
                }

                if (i == Context.Length - 1)
                {
                    if (storeToCondition)
                        ElementsMeetCondition.Add(new PartialStringIndex
                        {
                            Context = temporary.ToString(),
                            RecomposeOrder = recomposeIndex,
                            StartIndex = startIndex,
                            EndIndex = i
                        });
                    else
                        ElementsDoNotMeetCondition.Add(new PartialStringIndex
                        {
                            Context = temporary.ToString(),
                            RecomposeOrder = recomposeIndex,
                            StartIndex = startIndex,
                            EndIndex = i
                        });
                }
            }
        }

        public static StringDestructure By(string Context, Predicate<char> ListSplitCondition)
        {
            return new StringDestructure(Context, ListSplitCondition);
        }
        public StringDestructure OnConditionMet(Func<string, int, int, string> func)
        {
            this.ActionsOnConditionMet.Add(func);
            return this;
        }
        public StringDestructure OnConditionNotMet(Func<string, int, int, string> func)
        {
            this.ActionsOnConditionNotMet.Add(func);
            return this;
        }
        public string Recompose()
        {
            foreach (var item in this.ElementsMeetCondition)
            {
                string partialString = item.Context;
                foreach (var action in this.ActionsOnConditionMet)
                {
                    //NOTE : Even if the context is altered, StartIndex and End Index are not recalculated
                    partialString = action(partialString, item.StartIndex, item.EndIndex);
                }
                item.Context = partialString;
            }

            foreach (var item in this.ElementsDoNotMeetCondition)
            {
                string partialString = item.Context;
                //NOTE : Even if the context is altered, StartIndex and End Index are not recalculated
                foreach (var action in this.ActionsOnConditionNotMet)
                {
                    partialString = action(partialString, item.StartIndex, item.EndIndex);
                }
                item.Context = partialString;
            }
            List<PartialStringIndex> allitems =
                this.ElementsMeetCondition.Concat(this.ElementsDoNotMeetCondition).ToList();

            Comparison<PartialStringIndex> comparison = (first, second) =>
            {
                return first.RecomposeOrder.CompareTo(second.RecomposeOrder);
            };

            allitems.Sort(comparison);
            return string.Concat(allitems.Select(x => x.Context));
        }
    }
}
