using ConsoleToDoList.App;
using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleToDoList
{
    partial class Action
    {
        private void Options()
        {
            ConsoleMenu menu = new ConsoleMenu(MainMenuStyle);
            menu.AutoBackKeyButton = ConsoleKey.Backspace;
            menu.add(new ConsoleColorString("Back"), menu.exitFunction);
            menu.add(new ConsoleColorString("Manual Save", ConsoleColor.Green), () =>
            {
                LogicCORE.Save();
                menu.exitFunction();
            });
            menu.add(new ConsoleColorString("Manual Load", ConsoleColor.Yellow), () =>
            {
                if (ConsoleConfirmAlert.Show("Are you sure ? You could lose unsaved changes.", ConsoleColor.Yellow))
                {
                    LogicCORE.Load();
                    menu.exitFunction();
                }
            });
            menu.add(new ConsoleColorString("Remove Tags", ConsoleColor.DarkYellow), removeTagsFromSystem);
            menu.add(new ConsoleColorString("Reset Data", ConsoleColor.Red), () =>
            {
                if (ConsoleConfirmAlert.Show("Are you sure ? You lose all data.", ConsoleColor.Red))
                {
                    LogicCORE.Core = new LogicCORE();
                    LogicCORE.Save();
                    menu.exitFunction();
                }
            });
            menu.show();
        }

        private void removeTagsFromSystem()
        {
            bool reload;
            List<Tag> tagList = LogicCORE.Core.TagsData.TagsList.OrderBy(x => x.TagName).ToList();

            do
            {
                reload = false;
                ConsoleMenu menu = new ConsoleMenu(ViewMenuStyle);
                menu.AutoBackKeyButton = ConsoleKey.Backspace;

                menu.MenuTitle = new ConsoleColorString("Remove Tags From System", ConsoleColor.Yellow);


                menu.add(new ConsoleColorString("Search"), () => 
                {
                    Console.Clear();
                    Console.Write("Tag Name : ");
                    string buff = Console.ReadLine();

                    if (!string.IsNullOrEmpty(buff))
                    {
                        Regex regex = new Regex($"{buff}");
                        tagList = tagList.Where(x => regex.IsMatch(x.TagName)).OrderBy(x => x.TagName).ToList();
                    }
                    reload = true; 
                    menu.exitFunction(); 
                });
                menu.add(new ConsoleColorString("Back\n"), menu.exitFunction);

                foreach (var item in tagList)
                {
                    menu.add(new ConsoleColorString(item.TagName, ConsoleColor.Yellow), () =>
                    {
                        if (ConsoleConfirmAlert.Show($"Are you sure you want to delete this tag ({item.TagName}) ?", ConsoleColor.Yellow))
                        {
                            LogicCORE.Core.TagsData.RemoveTag(item,true);
                            tagList.Remove(item);
                            if (ConsoleConfirmAlert.Show($"Delete from all Task ?", ConsoleColor.Yellow))
                            {
                                TagController.RemoveTagFromAllNodes(LogicCORE.Core.TasksData, item);
                            }
                            reload = true;
                            menu.exitFunction();
                        }
                    });
                }
                menu.show();
            } while (reload);
        }
    }
}
