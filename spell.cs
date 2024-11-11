using System;

namespace CardGame
{
    class light_spell : Card
    {
        public int Health { get; set; }
        public light_spell(int health)
        {
            Health = health;
            Type_of_card = type_of_card.light_spell;
        }
        public override void use_card(Creatures enemy)
        {
            enemy.Health += Health;
            Console.WriteLine($"{enemy.Type_of_creatures} Вылечен на {Health} HP");
        }
        public override void print_info()
        {
            Console.WriteLine($"Type: {Type_of_card}           Heal:3");
        }
    }
    class dark_spell : Card
    {
        public int Damage { get; set; }
        public dark_spell(int damage)
        {
            Damage = damage;
            Type_of_card = type_of_card.dark_spell;
        }
        public override void use_card(Creatures enemy)
        {
            enemy.Health -= Damage;
            Console.WriteLine($"{enemy.Type_of_creatures} получил урон на {Damage} HP");
        }
        public override void print_info()
        {
            Console.WriteLine($"Type: {Type_of_card}            Damage:3");
        }
    }
}
