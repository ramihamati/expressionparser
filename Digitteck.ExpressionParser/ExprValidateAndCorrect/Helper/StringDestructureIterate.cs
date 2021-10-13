using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Helper
{
    /// <summary>
    /// Creates 2 lists based on condition passed. Partial strings that meet the condition
    /// will be held in ElementsMeetConditionList
    /// </summary>
    public class StringDestructureIterate
    {
        public class PartialStringIndex
        {
            public string Context;
            public int StartIndex;
            public int EndIndex;
        }

        public List<PartialStringIndex> ElementsMeetCondition;
        public List<PartialStringIndex> ElementsDoNotMeetCondition;
        /// When iterating items in the list ElementsMeetCondition invoke these methods
        List<Action<string, int, int>> ActionsOnConditionMet;
        /// When iterating items in the list ElementsMeetCondition invoke these methods
        List<Action<string, int, int>> ActionsOnConditionNotMet;
        private bool stopIteration = false;

        private StringDestructureIterate(string Context, Predicate<char> predicate)
        {
            ElementsMeetCondition = new List<PartialStringIndex>();
            ElementsDoNotMeetCondition = new List<PartialStringIndex>();
            ActionsOnConditionMet = new List<Action<string, int, int>>();
            ActionsOnConditionNotMet = new List<Action<string, int, int>>();

            StringBuilder temporary = new StringBuilder();

            bool storeToCondition = false;
            int startIndex = 0;

            for (int i = 0; i < Context.Length; i++)
            {
                char ch = Context[i];
                //if this is the first item - then we add to temporary
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
                //if not the first item, we can add items to collection if status changes
                else {
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
                                StartIndex = startIndex,
                                EndIndex = i - 1
                            });
                            startIndex = i;
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
                                StartIndex = startIndex,
                                EndIndex = i - 1
                            });
                            startIndex = i;
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
                //this must always check
                if (i == Context.Length - 1)
                {
                    if (storeToCondition)
                        ElementsMeetCondition.Add(new PartialStringIndex
                        {
                            Context = temporary.ToString(),
                            StartIndex = startIndex,
                            EndIndex = i
                        });
                    else
                        ElementsDoNotMeetCondition.Add(new PartialStringIndex
                        {
                            Context = temporary.ToString(),
                            StartIndex = startIndex,
                            EndIndex = i
                        });
                }
            }
        }

        public static StringDestructureIterate By(string Context, Predicate<char> ListSplitCondition)
        {
            return new StringDestructureIterate(Context, ListSplitCondition);
        }

        public StringDestructureIterate ForEachIfConditionMet(Action<string, int, int> Action)
        {
            this.ActionsOnConditionMet.Add(Action);
            return this;
        }

        public StringDestructureIterate ForEachIfConditionNotMet(Action<string, int, int> Action)
        {
            this.ActionsOnConditionNotMet.Add(Action);
            return this;
        }

        public void StopIterating() => stopIteration = true;

        public void Eval()
        {
            foreach (var item in this.ElementsMeetCondition)
            {
                foreach (var action in this.ActionsOnConditionMet)
                {
                    action(item.Context, item.StartIndex, item.EndIndex);
                    if (stopIteration) break;
                }
                if (stopIteration) break;
            }

            foreach (var item in this.ElementsDoNotMeetCondition)
            {
                foreach (var action in this.ActionsOnConditionNotMet)
                {
                    action(item.Context, item.StartIndex, item.EndIndex);
                    if (stopIteration) break;
                }
                if (stopIteration) break;
            }
        }
    }
}
