namespace TicTacToe
{
    class Menu
    {
        public static string ShowMenu(List<string> menuItems)
        {
            Console.WriteLine("Please make a Choice:");

            for (int i = 0; i < menuItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {menuItems[i]}");
            }

            while (true)
            {
                Console.Write("Enter the number of your choice: ");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                char keyChar = keyInfo.KeyChar;
                string input = keyChar.ToString();
                Console.WriteLine();


                if (int.TryParse(input, out int choice))
                {
                    if (choice >= 1 && choice <= menuItems.Count)
                    {
                        return menuItems[choice - 1];
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice, try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }
        }

        
    }
}


