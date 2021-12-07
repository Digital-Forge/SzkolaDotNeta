using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleToDoList.App
{
    public static partial class ReportsBuilder
    {
        public static void TagReport()
        {
            bool reload;
            List<Tag> tagList = LogicCORE.Core.TagsData.TagsList.OrderBy(x => x.TagName).ToList();

            do
            {
                reload = false;
                ConsoleMenu menu = new ConsoleMenu(ViewMenuStyle);
                menu.AutoBackKeyButton = ConsoleKey.Backspace;

                menu.MenuTitle = new ConsoleColorString("Remove Tags From System", ConsoleColor.Yellow);


                menu.add(new ConsoleColorString("Serch"), () =>
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
                            generateReportOfTag(item);
                            menu.exitFunction();
                        }
                    });
                }
                menu.show();
            } while (reload);
        }

        private static void generateReportOfTag(Tag tag)
        {
            levelOfProgress = 0;
            partsProgress = 8;
            printProgress();




            




        }
    }
}
