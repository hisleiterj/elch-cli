using System;
using System.Collections.Generic;
using System.Linq;

namespace Elchwinkel.CLI
{
    public class Menu
    {
        public class Item
        {
            public string Label { get; set; }
            public object Value { get; set; }

            public Item(string label, object value)
            {
                Label = label;
                Value = value;
            }
        }
        private Item[] _items { get; }
        private int _selectedItemIndex = 0;
        private bool _itemIsSelected;
        private bool _escapeRequested;

        public bool AllowEscape { get; set; } = true;
        public bool HideMenuAfterClosing { get; set; } = true;

        public Menu(IEnumerable<Item> menuItems) 
            => _items = menuItems.ToArray();
        public Menu(IEnumerable<string> options) 
            => _items = options.Select(s => new Item(s, s)).ToArray();

        public object Show()
        {
            _itemIsSelected = false;
            _escapeRequested = false;
            _StartConsoleDrawingLoopUntilInputIsMade();
            return _escapeRequested ? null : _items[_selectedItemIndex].Value;
        }
        public static object Show(params string[] options)
        {
            var menu = new Menu(options);
            return menu.Show();
        }
        private static void _ClearLine(int line)
        {
            Console.SetCursorPosition(0, line);
            _ClearCurrentConsoleLine();
        }
        private static void _ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        private void _StartConsoleDrawingLoopUntilInputIsMade()
        {
            int topOffset = Console.CursorTop;
            int bottomOffset = 0;
            ConsoleKeyInfo kb;
            var cursorVisibilityBackup = Console.CursorVisible;
            Console.CursorVisible = false;
            while (!_itemIsSelected && !_escapeRequested)
            {
                for (int i = 0; i < _items.Length; i++)
                {
                    _WriteConsoleItem(i, _selectedItemIndex);
                }

                bottomOffset = Console.CursorTop;
                kb = Console.ReadKey(true);
                _HandleKeyPress(kb.Key);

                Console.SetCursorPosition(0, topOffset);
            }

            if (HideMenuAfterClosing)
            {
                for (int i = 1; i <= _items.Length; i++)
                {
                    _ClearLine(bottomOffset - i);
                }
            }
            else
            {
                Console.SetCursorPosition(0, bottomOffset);
            }
            Console.CursorVisible = cursorVisibilityBackup;
        }

        private void _HandleKeyPress(ConsoleKey pressedKey)
        {
            switch (pressedKey)
            {
                case ConsoleKey.UpArrow:
                    _selectedItemIndex = (_selectedItemIndex == 0) ? _items.Length - 1 : _selectedItemIndex - 1;
                    break;

                case ConsoleKey.DownArrow:
                    _selectedItemIndex = (_selectedItemIndex == _items.Length - 1) ? 0 : _selectedItemIndex + 1;
                    break;

                case ConsoleKey.Enter:
                    _itemIsSelected = true;
                    break;
                case ConsoleKey.Escape:
                    if(AllowEscape)
                        _escapeRequested = true;
                    break;
            }
        }

        private void _WriteConsoleItem(int itemIndex, int selectedItemIndex)
        {
            var backgroundColorBackup = Console.BackgroundColor;
            var foregroundColorBackup = Console.ForegroundColor;
            if (itemIndex == selectedItemIndex)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.WriteLine(" {0,-20}", this._items[itemIndex].Label);
            Console.BackgroundColor = backgroundColorBackup;
            Console.ForegroundColor = foregroundColorBackup;
        }
    }
}