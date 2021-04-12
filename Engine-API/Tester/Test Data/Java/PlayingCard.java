


public abstract class PlayingCard {
    public int value;
    public int rank;
    public String suit;
    
    @Override
    public abstract String toString();
    
    /**
     * @return the value of the card
     */
    public int getValue(){
        return value;
    }
    
    /**
     * @return the rank of the card
     */
    public int getRank(){
        return rank;
    }
    
    /**
     * @return suit of the card
     */
    public String getSuit(){
        return suit;
    }
}
