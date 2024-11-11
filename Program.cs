using System;
using System.Collections.Generic;
using System.Linq;


namespace CardGame
{
    class Program
    {

        static List<string> type_of_creatures = new List<string> { "undead", "elemental", "beast", "murlok" };
        static List<string> type_of_spell = new List<string> { "dark_spell", "light_spell"};

        static Random rng = new Random();
        static int number_card_player;
        static int number_card_enemy;
        static string number_card_str;
        static int heal_card_index;
        static int count_of_move = 0;

        //"Рука" игрока и соперника
        static List<Card> hand_player = new List<Card> { };
        static List<Card> hand_enemy = new List<Card> { };

        //Колоды игрока и соперника
        static List<Card> deck_player = new List<Card> { };
        static List<Card> deck_enemy = new List<Card> { };

        //Стол игрока и соперника
        static List<Card> board_player = new List<Card> { };
        static List<Card> board_enemy = new List<Card> { };

        //Главные карты игрока и соперника
        static Creatures main_player = new Creatures(20, 1);
        static Creatures main_enemy = new Creatures(20, 1);

        //"Смерть карты"
        static void Card_died(Creatures card, string type_card)
        {
            //Проверка если карта была основной
            if(card.Type_of_creatures == "MAIN_CARD" && type_card == "enemy")
            {
                Console.WriteLine("ВЫ ВЫИГРАЛИ");
                Console.ReadKey();
                Environment.Exit(0);
            }
            if(card.Type_of_creatures == "MAIN_CARD" && type_card == "player")
            {  
                Console.WriteLine("ВЫ ПРОИГРАЛИ");
                Console.ReadKey();
                Environment.Exit(0);
            }

            switch (type_card)
            {
                case "enemy":
                    Console.WriteLine($"\nКарта соперника {card.Type_of_creatures} убита");
                    board_enemy.Remove(card);
                    break;
                case "player":
                    Console.WriteLine($"\nВаша карта {card.Type_of_creatures} убита");
                    board_player.Remove(card);
                    break;
            }

        }

        //Вывод информации о состоянии стола
        static void Print_info()
        {
            Console.WriteLine("\n\n----------Стол соперника----------\n");
            foreach (var card in board_enemy)
            {
                card.print_info();
            }

            Console.WriteLine("\n-------------Ваш стол-------------\n");
            foreach (var card in board_player)
            {
                card.print_info();
            }
            Console.WriteLine("\n\n");
        }

        //Берём карты из колоды
        static void Get_card_in_deck()
        {
            //Если колода не пуста, то берет карту
            if (deck_enemy.Count != 0)
            {
                hand_enemy.Add(deck_enemy[0]);
                deck_enemy.Remove(deck_enemy[0]);
            }
            
            if (deck_player.Count != 0)
            {
                hand_player.Add(deck_player[0]);
                deck_player.Remove(deck_player[0]);
            }

        }

        //Ход противника (расстановка карт)
        static void Enemy_turn()
        {
            Console.WriteLine("-------Ход противника-------\n");

            
            //Если у противника есть карта на руке, то он ее выкладывает
            if (hand_enemy.Count != 0)
            {
                //Генерит рандомную карту которой походит противник
                number_card_enemy = rng.Next(0, hand_enemy.Count);

                //Выбор действия в зависимости от типа карты
                switch (hand_enemy[number_card_enemy].Type_of_card)
                {
                    case type_of_card.creature:
                        //Противник выкладывает карту на стол если это существо
                        board_enemy.Add(hand_enemy[number_card_enemy]);
                        Console.WriteLine($"Соперник выложил карту {((Creatures)hand_enemy[number_card_enemy]).Type_of_creatures}");
                        hand_enemy.Remove(hand_enemy[number_card_enemy]);
                        break;

                    case type_of_card.dark_spell:

                        //Выбирает на какую карту игрока использовать спел
                        number_card_player = rng.Next(0, board_player.Count);

                        hand_enemy[number_card_enemy].use_card((Creatures)(board_player[number_card_player]));
                        Console.WriteLine($"Соперник использовал dark_spell на {((Creatures)board_player[number_card_player]).Type_of_creatures}");

                        //Если спел убил существо игрока
                        if (((Creatures)board_player[number_card_player]).Health <= 0)
                        {
                            Card_died(((Creatures)board_player[number_card_player]), "player");
                        }

                        //Удаляем спел из руки соперника
                        hand_enemy.Remove(hand_enemy[number_card_enemy]);

                        break;
                    //Если light_spell то хилит свою карту
                    case type_of_card.light_spell:

                        //Выбирает какую карту отхилить
                        heal_card_index = rng.Next(0, board_enemy.Count);

                        hand_enemy[number_card_enemy].use_card((Creatures)board_enemy[heal_card_index]);
                        Console.WriteLine($"Соперник использовал light_spell на {((Creatures)board_enemy[heal_card_index]).Type_of_creatures}");

                        //Удаляем спел из руки соперника
                        hand_enemy.Remove(hand_enemy[number_card_enemy]);
                        break;
                }
            }

            Console.WriteLine("\n\n");

        }

        //Ход противника (атака)
        static void Enemy_attack()
        {
            //Считаем сколько раз может походить
            for (int i = 0; i < board_enemy.Count; i++)
            {
                count_of_move += ((Creatures)board_enemy[i]).Ready_to_attack;
            }

            while (count_of_move > 0)
            {

                Print_info();

                //Соперник выбирает карту для атаки
                number_card_enemy = rng.Next(0, board_enemy.Count);


                //Соперник выбирает карту игрока для атаки
                number_card_player = rng.Next(0, board_player.Count);

                Console.WriteLine($"{((Creatures)board_enemy[number_card_enemy]).Type_of_creatures} атаковал {((Creatures)board_player[number_card_player]).Type_of_creatures}");
                ((Creatures)board_enemy[number_card_enemy]).use_card((Creatures)board_player[number_card_player]);

                //Если атака убила карту игрока
                if (((Creatures)board_player[number_card_player]).Health <= 0)
                {
                    Card_died(((Creatures)board_player[number_card_player]), "player");
                }

                //Опускаем флаг готовности к атаке походившей карты
                ((Creatures)board_enemy[number_card_enemy]).Ready_to_attack = 0;
                count_of_move--;
            }

            //Возвращаем готовность карт на доске к атаке перед след ходом
            for (int i = 0; i < board_enemy.Count; i++)
            {
                ((Creatures)board_enemy[i]).Ready_to_attack = 1;
            }
        }

        //Ход игрока (расстанвока карт)
        static void Player_turn()
        {
            Console.WriteLine("---------Ход игрока---------\n");

            //Если на руке остались карты
            if (hand_player.Count != 0)
            {
                foreach (var item in hand_player)
                {
                    item.print_info();
                }
                Console.WriteLine("0 - не использовать");
                Console.WriteLine("\nВаша рука, введите номер карты которую хотите использовать ");


                //Игрок выбирает карту
                number_card_str = Console.ReadLine();
                number_card_player = Convert.ToInt32(number_card_str) - 1;


                if (number_card_player != -1)
                {
                    //Выбор действия в зависимости от типа карты
                    switch (hand_player[number_card_player].Type_of_card)
                    {
                        case type_of_card.creature:
                            //Выкладываем карту существа на стол
                            board_player.Add(hand_player[number_card_player]);
                            hand_player.Remove(hand_player[number_card_player]);
                            break;

                        case type_of_card.dark_spell:

                            foreach (var card in board_enemy)
                            {
                                card.print_info();
                            }
                            Console.WriteLine("Выберите карту соперника : ");

                            //Игрок выбирает карту соперника
                            number_card_str = Console.ReadLine();
                            number_card_enemy = Convert.ToInt32(number_card_str) - 1;

                            hand_player[number_card_player].use_card((Creatures)board_enemy[number_card_enemy]);

                            //Если спел убил карту соперника
                            if (((Creatures)board_enemy[number_card_enemy]).Health <= 0)
                            {
                                Card_died(((Creatures)board_enemy[number_card_enemy]), "enemy");
                            }

                            //Удаляем спел из руки игрока
                            hand_player.Remove(hand_player[number_card_player]);
                            break;

                        case type_of_card.light_spell:

                            foreach (var card in board_player)
                            {
                                card.print_info();
                            }
                            Console.WriteLine("Выберите свою карту для лечения: ");

                            //Игрок выбирает карту для лечения
                            number_card_str = Console.ReadLine();
                            number_card_enemy = Convert.ToInt32(number_card_str) - 1;

                            hand_player[number_card_player].use_card((Creatures)board_player[number_card_enemy]);

                            //Удаляем спел из руки игрока
                            hand_player.Remove(hand_player[number_card_player]);
                            break;
                    }
                }
            }

        }

        //Ход игрока (атака)
        static void Player_attack()
        {
            //Считаем сколько раз можем походить
            for (int i = 0; i < board_player.Count; i++)
            {
                count_of_move += ((Creatures)board_player[i]).Ready_to_attack;
            }

            while (count_of_move > 0)
            {

                Print_info();


                //Выводим карты игрока которыми он может атаковать
                foreach (var card in board_player)
                {
                    if (((Creatures)card).Ready_to_attack == 1)
                    {
                        card.print_info();
                    }
                }

                Console.Write("Выберите свою карту для атаки: ");

                //Игрок выбирает карту для хода
                number_card_str = Console.ReadLine();
                number_card_player = Convert.ToInt32(number_card_str) - 1;

                foreach (var card in board_enemy)
                {
                    card.print_info();
                }
                Console.Write("Выберите карту соперника для атаки: ");

                //Игрок выбирает карту соперника для атаки
                number_card_str = Console.ReadLine();
                number_card_enemy = Convert.ToInt32(number_card_str) - 1;

                ((Creatures)board_player[number_card_player]).use_card((Creatures)board_enemy[number_card_enemy]);

                //Если атака убила карту соперника
                if (((Creatures)board_enemy[number_card_enemy]).Health <= 0)
                {
                    Card_died(((Creatures)board_enemy[number_card_enemy]), "enemy");
                }

                //Опускаем флаг готовности к атаке походившей карты
                ((Creatures)board_player[number_card_player]).Ready_to_attack = 0;
                count_of_move--;
            }

            //Возвращаем готовность карт на доске к атаке перед след ходом
            for (int i = 0; i < board_player.Count; i++)
            {
                ((Creatures)board_player[i]).Ready_to_attack = 1;
            }
        }

        static void Main(string[] args)
        {

            //Генерируем колоды
            foreach (var item in type_of_creatures)
            {
                for (int i = 1; i < 5; i++)
                {
                    for (int j = 1; j < 5; j++)
                    {
                        if (item == "undead") {
                            deck_player.Add(new Undead(i, j));
                        }
                        if (item == "elemental")
                        {
                            deck_player.Add(new Elemental(i, j));
                        }
                        if (item == "beast")
                        {
                            deck_player.Add(new Beast(i, j));
                        }
                        if (item == "murlok")
                        {
                            deck_player.Add(new Murlok(i, j));
                        }

                    }
                }
            }
            foreach (var item in type_of_spell)
            {
                for (int i = 0; i < 3; i++)
                {
                    
                    if (item == "dark_spell")
                    {
                        deck_player.Add(new dark_spell(3));
                    }
                    if (item == "light_spell")
                    {
                        deck_player.Add(new light_spell(3));
                    }
                    
                }
            }


            //Мешаем карты
            deck_player = deck_player.OrderBy(x => rng.Next()).ToList();
            deck_enemy = deck_player.OrderBy(x => rng.Next()).ToList();
            
            //Начало игры (каждый берет по 4 карты)
            for (int i = 1; i < 5; i++)
            {
                hand_player.Add(deck_player[i]);
                deck_player.Remove(deck_player[i]);
                hand_enemy.Add(deck_enemy[i]);
                deck_enemy.Remove(deck_enemy[i]);
            }

            //Выкладываем на стол главные карты игрока и соперника
            board_player.Add(main_player);
            board_enemy.Add(main_enemy);

            //Главный цикл игровой схватки 
            while (true) 
            {
                Print_info();

                Enemy_turn();
                Enemy_attack();

                Print_info();

                Player_turn();
                Player_attack();

                Get_card_in_deck();
                Console.Clear();

            }  
        }
    }
}

    