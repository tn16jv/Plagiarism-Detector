
import java.util.Comparator;


public class CardCompare implements Comparator<PlayingCard>{

    @Override
    public int compare(PlayingCard o1, PlayingCard o2) {
        if(o1.rank>o2.rank) return 1;
        else if(o1.rank<o2.rank)return -1;
        else return 0;
    }
    
}
