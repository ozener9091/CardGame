using System;

namespace CardGame
{
    public class Creatures : Card
    {
        public int Damage { set; get; }
        public int Health { set; get; }
        public int Ready_to_attack { set; get; }
        public string Type_of_creatures { set; get; }


        public Creatures(int health, int damage)
        {
            Damage = damage;
            Health = health;
            Ready_to_attack = 0;
            Type_of_card = type_of_card.creature;
            Type_of_creatures = "MAIN_CARD";
        }


        public override void use_card(Creatures enemy)
        {
            enemy.Health -= Damage;
        }
   

        public override void print_info()
        {
            Console.WriteLine($"Type: {Type_of_creatures}    HP: {Health}   Damage: {Damage}");
        }

    }

    class Undead : Creatures
    {
        private int reborn;

        public Undead(int health, int damage) : base(health, damage)
        {
            Type_of_creatures = "undead";
            reborn = 1; 
        }
        public void Reborned()
        {
            if (Health < 0 && reborn == 1)
            {
                Health = 1;
                reborn = 0;
                Console.WriteLine("Undead возродился");
            }
        }
        public override void print_info()
        {
            Console.WriteLine($"Type: {Type_of_creatures}       HP: {Health}    Damage: {Damage}");
        }
    }
    class Elemental : Creatures
    {
        public Elemental(int health, int damage) : base(health, damage)
        {
            Type_of_creatures = "elemental";
            Health = health * 2;
        }
        public override void print_info()
        {
            Console.WriteLine($"Type: {Type_of_creatures}    HP: {Health}    Damage: {Damage}");
        }
    }
    class Beast : Creatures
    {
        public Beast(int health, int damage) : base(health, damage)
        {
            Type_of_creatures = "beast";
            Damage = damage * 2;
        }
        public override void print_info()
        {
            Console.WriteLine($"Type: {Type_of_creatures}        HP: {Health}    Damage: {Damage}");
        }
    }

    class Murlok : Creatures
    {
        public Murlok(int health, int damage) : base(health, damage)
        {
            Type_of_creatures = "murlok";
        }
        public override void print_info()
        {
            Console.WriteLine($"Type: {Type_of_creatures}       HP: {Health}    Damage: {Damage}");
        }
    }

}