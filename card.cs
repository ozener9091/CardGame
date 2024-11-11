
namespace CardGame
{
    public enum type_of_card
    {
        creature,
        dark_spell,
        light_spell
    }

    public abstract class Card
    {
        public type_of_card Type_of_card { get; set; }
        public virtual void print_info(){}
        public virtual void use_card(Creatures enemy){}

    }
}