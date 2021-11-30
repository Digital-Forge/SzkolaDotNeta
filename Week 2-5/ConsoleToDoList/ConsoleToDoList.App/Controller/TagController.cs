using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace ConsoleToDoList.App
{
    public static class TagController
    {
        public static ConsoleMenu.MenuStyle MenuStyle = null;
        private static List<Tag> tagList;

        public static void AddTags(TaskHook hook)
        {
            bool reload;
            tagList = LogicCORE.Core.TagsData.TagsList.OrderBy(x => x.TagName).ToList();

            do
            {
                reload = false;
                ConsoleMenu menu = new ConsoleMenu(MenuStyle);
                menu.AutoBackKeyButton = ConsoleKey.Backspace;

                hook.TagsBag.SortByTagName();
                hook.TagsBag.SortByLock();

                menu.MenuTitle = new ConsoleColorString("Tags :");
                foreach (var item in hook.TagsBag.cellsOfTagsList)
                {
                    if (item.Lock) menu.MenuTitle.AddText($" {item.Tag.TagName}", ConsoleColor.Red);
                    else           menu.MenuTitle.AddText($" {item.Tag.TagName}", ConsoleColor.Green);
                }

                menu.add(new ConsoleColorString("Serch"), () => { serchTagByName(); reload = true; menu.exitFunction(); });
                menu.add(new ConsoleColorString("New Tag"), () => { newAddTag(hook); reload = true; menu.exitFunction(); });
                menu.add(new ConsoleColorString("Back\n"), menu.exitFunction);

                foreach (var item in tagList)
                {
                    menu.add(new ConsoleColorString(item.TagName, ConsoleColor.Yellow), () => { applyTagToUpEndNode(item,hook); reload = true; menu.exitFunction(); });
                }
                menu.show();
            } while (reload);
        }

        public static void RemoveTags(TaskHook hook)
        {
            bool reload;
            tagList = hook.TagsBag.cellsOfTagsList.Where(x => x.Lock == false).Select(x => x.Tag).OrderBy(x => x.TagName).ToList();

            do
            {
                reload = false;
                ConsoleMenu menu = new ConsoleMenu(MenuStyle);
                menu.AutoBackKeyButton = ConsoleKey.Backspace;

                hook.TagsBag.SortByTagName();
                hook.TagsBag.SortByLock();

                menu.MenuTitle = new ConsoleColorString("RemoveTags", ConsoleColor.Yellow);


                menu.add(new ConsoleColorString("Serch"), () => { serchTagByName(); reload = true; menu.exitFunction(); });
                menu.add(new ConsoleColorString("Back\n"), menu.exitFunction);

                foreach (var item in tagList)
                {
                    menu.add(new ConsoleColorString(item.TagName, ConsoleColor.Yellow), () => 
                    { 
                        if (ConsoleConfirmAlert.Show($"Are you sure you want to delete this tag ({item.TagName}) ?"))
                        {
                            removeTagToUpEndNode(item, hook);
                            tagList = hook.TagsBag.cellsOfTagsList.Where(x => x.Lock == false).Select(x => x.Tag).OrderBy(x => x.TagName).ToList();
                            reload = true;
                            menu.exitFunction();
                        } 
                    });
                }
                menu.show();
            } while (reload);
        }

        public static void ApplyTagsToUpNode(TaskHook hook)
        {
            if (hook.Node?.LastNode?.Data != null)
            {
                (hook.Node.LastNode.Data as TaskHook).TagsBag.AddTag(hook.TagsBag, true);
            }

        }

        private static void serchTagByName()
        {
            Console.Clear();
            Console.Write("Tag Name : ");
            string buff = Console.ReadLine();

            if (!string.IsNullOrEmpty(buff))
            {
                Regex regex = new Regex($"{buff}");
                tagList = tagList.Where(x => regex.IsMatch(x.TagName)).OrderBy(x => x.TagName).ToList();
            }
        }

        private static void applyTagToUpEndNode(Tag tag, TaskHook hook, bool _lock = false)
        {
            if (hook != null)
            {
                hook.TagsBag.AddTag(tag, _lock);

                if (hook.Node?.LastNode != null)
                {
                    applyTagToUpEndNode(tag, hook.Node.LastNode.Data as TaskHook, true);
                }
            }
        }

        private static void newAddTag(TaskHook hook)
        {
            Console.Clear();
            Console.Write("New Tag Name : ");
            string buff = Console.ReadLine();

            if (string.IsNullOrEmpty(buff))
            {
                ConsoleAlert.Show("Empty name !!!", ConsoleColor.Red);
                return;
            }

            if (LogicCORE.Core.TagsData.TagsList.Exists(x => x.TagName.ToLower() == buff.ToLower()))
            {
                ConsoleAlert.Show("Tag exist -> tag add", ConsoleColor.Yellow);
                applyTagToUpEndNode(LogicCORE.Core.TagsData.TagsList.Find(x => x.TagName.ToLower() == buff.ToLower()), hook);
            }
            else
            {
                Tag tag = new Tag(buff);
                LogicCORE.Core.TagsData.AddTag(tag);
                applyTagToUpEndNode(tag, hook);
            }
        }

        private static void removeTagToUpEndNode(Tag tag, TaskHook hook, bool first = true)
        {
            if (hook != null)
            {
                if (first)
                {
                    hook.TagsBag.RemoveTag(tag, true);
                    if (hook.Node?.LastNode?.Data != null)
                    {
                        removeTagToUpEndNode(tag, hook, false);
                    }
                }
                else
                {
                    if (hook.Node?.LastNode?.Data != null)
                    {
                        TaskHook parent = hook.Node?.LastNode?.Data as TaskHook;
                        bool findStatus = false;

                        foreach (var node in parent.Node.NextNodes)
                        {
                            if (node.Data as TaskHook != hook)
                            {
                                if (node.Data != null)
                                {
                                    if ((node.Data as TaskHook).TagsBag.TagsList.Exists(x => x.TagName.ToLower() == tag.TagName.ToLower()))
                                    {
                                        findStatus = true;
                                        break;
                                    }
                                }
                            }
                            else continue;
                        }

                        if (!findStatus)
                        {
                            parent.TagsBag.RemoveTag(tag, true);
                            removeTagToUpEndNode(tag, parent, false);
                        }
                    }
                }
            }
        }
    }
}
