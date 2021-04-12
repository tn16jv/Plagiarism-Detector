


public class NumberCard extends PlayingCard {
    
    /**
     * constructor
     * @param suit the suit of the card to be created
     * @param rank the rank of the card ie 2,3,4...
     */
    public NumberCard(String suit,int rank){
        super.suit = suit;
        super.rank = rank;
        super.value = rank;
    }

    @Override
    public String toString() {
        return "["+String.valueOf(this.rank)+this.suit+"]";
    }
    
}
