
public class Ace extends PlayingCard {

    /**
     * constructor
     * @param suit of the card
     * Ace card is always set to value and rank 1
     */
    public Ace(String suit){
        super.suit = suit;
        super.rank = 1;
        super.value = 1;
    }
    
    @Override
    public String toString() {
        return "[A"+this.suit+"]";
    }
    
}
