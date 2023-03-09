using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateboardDataManager : MonoBehaviour {

    void TotalPrice() {
        SkateboardData.totalPrice = SkateboardData.deckPrice + SkateboardData.trackPrice + SkateboardData.wheelsPrice +
                                    SkateboardData.bearingsPrice + SkateboardData.hardwarePrice;
    }

    public void ResetPrice() {
        SkateboardData.totalPrice = SkateboardData.deckPrice = SkateboardData.trackPrice = SkateboardData.wheelsPrice =
                                    SkateboardData.bearingsPrice = SkateboardData.hardwarePrice = 0;
    }

    public void SkateboardSetInformation(string part, string name, float price) {
        if(part == SkateboardParts.Deck.ToString()) {
            SkateboardData.skateboardDeck = name;
            SkateboardData.deckPrice = price;
        } else if (part == SkateboardParts.Track.ToString()) {
            SkateboardData.skateboardTrack = name;
            SkateboardData.trackPrice = price;
        } else if (part == SkateboardParts.Wheels.ToString()) {
            SkateboardData.skateboardWheels = name;
            SkateboardData.wheelsPrice = price;
        } else if (part == SkateboardParts.Bearings.ToString()) {
            SkateboardData.skateboardBearings = name;
            SkateboardData.bearingsPrice = price;
        } else if (part == SkateboardParts.Hardware.ToString()) {
            SkateboardData.skateboardHardware = name;
            SkateboardData.hardwarePrice = price;
        }

        TotalPrice();
    }
}