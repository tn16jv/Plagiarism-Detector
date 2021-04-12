


public class FaceCard extends PlayingCard{
    
    /**
     * constructor
     * @param suit of the card
     * @param rank of the card ie 11,12,13
     * value is always set to 10
     */
    public FaceCard(String suit,int rank){
        super.suit = suit;
        super.rank = rank;
        super.value = 10;
    }

    @Override
    public String toString() {
        switch(this.rank) {
        	case 11:
        		return "[J"+ this.suit+"]";
        	case 12:
        		return "[Q"+ this.suit+"]";
        	case 13:
        		return "[K"+ this.suit+"]";
        	default:
        		return null;
        }
    }
    
}
