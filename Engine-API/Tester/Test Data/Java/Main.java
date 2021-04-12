

import java.util.LinkedList;
import java.util.Scanner;

public class Main {

   
    public static void main(String[] args) {
    	
    	Scanner reader = new Scanner(System.in);
    	
        
        Deck deck;
        CribbageHand pOne;
        CribbageHand pTwo;
        CribbageHand crib;
        PlayingCard turnUp;
        
        /*LinkedList<PlayingCard> list = new LinkedList<PlayingCard>();
        list.add(new NumberCard("H",6));
        list.add(new NumberCard("S",5));
        list.add(new FaceCard("D",12));
        list.add(new FaceCard("S",13));
        PlayingCard a = new Ace("C");
        pOne = new CribbageHand(a,false,list);
        System.out.println(pOne.getTally());*/
        
        
        while(true) {
        	deck = new Deck();
        	System.out.println("A deck is created:");
        	System.out.println();
        	System.out.println(deck.toString());
        	System.out.println();
        	
        	System.out.println("The deck has been shuffled:");
        	deck.shuffle();
        	System.out.println();
        	System.out.println(deck.toString());
        	System.out.println();
        	
        	System.out.println("A turn up card has been drawn:");
        	turnUp = deck.deal();
        	System.out.println();
        	System.out.println(turnUp.toString());
        	System.out.println();
        	
        	System.out.println("The hands have been drawn(two hands and a crib):");
        	System.out.println();
        	
        	System.out.println("Hand one:");
        	pOne = new CribbageHand(turnUp,false,deck.draw(4));
        	System.out.println(pOne.toString()+"|"+turnUp.toString()+"     Score = "+pOne.getTally());
        	System.out.println();
        	
        	System.out.println("Hand two:");
        	pTwo = new CribbageHand(turnUp,false,deck.draw(4));
        	System.out.println(pTwo.toString()+"|"+turnUp.toString()+"     Score = "+pTwo.getTally());
        	System.out.println();
        	
        	System.out.println("Crib:");
        	crib = new CribbageHand(turnUp,true,deck.draw(4));
        	System.out.println(crib.toString()+"|"+turnUp.toString()+"     Score = "+crib.getTally());
        	System.out.println();
        	System.out.println("Would you like to run again? y/n");
        	
        	String n = reader.next();
        	if(n.toLowerCase().equals("n")) break;
        	else continue;
        		
        	
        	
        }
    }
    
}
