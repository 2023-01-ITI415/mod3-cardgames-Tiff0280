using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This enum defines the variable type eCardState with four named values.     
namespace NewProspector
{
    public enum eCardState { drawpile, mine, target, discard }

    public class CardGolf : Card
    {
        // Make CardProspector extend Card        
        [Header("Dynamic: CardGolf")]
        public eCardState state = eCardState.drawpile;
        // The hiddenBy list stores which other cards will keep this one face down
        public List<CardGolf> hiddenBy = new List<CardGolf>();
        // The layoutID matches this card to the tableau JSON if it�s a tableau card
        public int layoutID;
        // The JsonLayoutSlot class stores information pulled in from JSON_Layout
        public JsonLayoutSlot layoutSlot;

        /// <summary>
        /// Informs the Prospector class that this card has been clicked.
        /// </summary>
        override public void OnMouseUpAsButton()
        {
            // Uncomment the next line to call the base class version of this method
            // base.OnMouseUpAsButton();                                       
            // Call the CardClicked method on the Prospector Singleton
            ProspectorNEW.CARD_CLICKED(this);
        }
    }
}