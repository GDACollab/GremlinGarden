VAR GREMLIN_NAME = "Default"
VAR SELECTED_RACE = "starter"
VAR WON_RACE = 0
VAR HAS_WON_RACE = 0

>>> UpdateInkVar(GREMLIN_NAME, gremlinToRace)
>>> UpdateInkVar(SELECTED_RACE, selectedRace)
>>> UpdateInkVar(WON_RACE, wonRace)
>>> UpdateInkVar(HAS_WON_RACE, hasWonRace)

>>> TextboxEnter(Default);
>>> !CharEnter(Sunny); CharEnter(Violet)

{ WON_RACE:
- 0:
    { HAS_WON_RACE:
        - 1:
            -> GenericPostRace
        - 0:
            -> UniqueLoseRace
    }
- 1:
    { HAS_WON_RACE:
        - 1:
            -> GenericPostRace
        - 0:
            -> UniqueWinRace
    }
}

-> END

=== UniqueWinRace ===
{ SELECTED_RACE:
- "Starter":
    // Race1Win
    Sunny: We won? #Sunny_Shock
    Sunny: {GREMLIN_NAME}, we actually won!!! #Sunny_BigSmile
    Violet: Congrats on your first win, Sunny! You and {GREMLIN_NAME} did really good out there! You two make a pretty great team! #Violet_BigSmile
    Sunny: Thank you! You and Jellybeans were also amazing out there! #Sunny_Neutral
    Violet: Thanks. But I still think we have room for improvement… #Violet_Disheartened
    Violet: … So we’ll try even harder so we win next time! #Violet_BigSmile
    Sunny: I can’t wait for our next race! #Sunny_BigSmile
- "Beginner":
    // Race2Win
    Sunny: I won again! I think I’m getting the hang of it. #Sunny_BigSmile
    Sunny: Oh! Violet! Thank you for the advice! I feel like I’m starting to build a really strong connection with {GREMLIN_NAME} now! #Sunny_Neutral
    Violet: That’s awesome! It’s great to see you two enjoying racing so much. #Violet_Talking
    Sunny: We’re doing so well, I think we might even go for the championship! #Sunny_Determined
    Violet: Is that so? Well you’re gonna have to give it your all, because you’re going to have to beat us too! #Violet_Neutral
- "Intermediate":
    // Race3Win
    Sunny: Great job out there, {GREMLIN_NAME}! #Sunny_BigSmile 
    Violet: {GREMLIN_NAME} is quite the racer, Sunny! You gave me and Jellybeans a real run for our money.
    Sunny: Well, me and {GREMLIN_NAME} aren’t stopping anytime soon! You’d best give it your all, too! #Sunny_Determined
    Violet: Of course! Where’s the fun in racing without some friendly rivalry? #Violet_Talking
    Violet: I’ll be looking forward to our next race! #Violet_BigSmile
    Sunny: Yeah! #Sunny_BigSmile
- "Expert":
    // Race4Win
    Sunny: We… We did it! #Sunny_Shock
    Violet: Wow… You’ve finally won the biggest race on the island! #Violet_Disheartened
    Violet: You were amazing out there! Hahaha, we haven’t had to try that hard in a race in a while! #Violet_BigSmile
    Violet: You’re a really tough opponent Sunny, and so was {GREMLIN_NAME}! #Violet_Neutral
    Sunny: Thank you! I don’t think I could’ve done it without you and Jellybeans, though. #Sunny_Neutral
    Sunny: Your advice and Jellybeans’ cuteness really helped me keep my head in the right space. #Sunny_BigSmile
    Violet: Aww, thank you. Now that you’ve won the biggest race, what are you going to do now, Sunny?  #Violet_Neutral
    Sunny: I’m not going to give up! My dream is to become the best Grom ever. I’ll train even harder and keep giving it my all, race after race. #Sunny_Determined
    Violet: That’s some amazing spirit you have.
    Violet: Alright! I won’t let you overtake me more than you have! Double training for us, Jellybeans! #Violet_Talking
    Violet: Just you wait, Sunny! #Violet_BigSmile
    Sunny: Good! I wouldn’t have it any other way. #Sunny_BigSmile
- else:
    -> GenericPostRace
}
-> END

=== UniqueLoseRace ===
{ SELECTED_RACE:
- "Starter":
    // Race1Lose
    Sunny: Oh no... we lost? #Sunny_Shock
    Violet: Hey, it’s OK! You don’t have to look so down. This was only your first race, not everyone gets the hang of it immediately. #Violet_Disheartened
    Violet: You should try learning more about {GREMLIN_NAME} and what they like; compatibility is really important when it comes to racing! #Violet_Neutral
    Sunny: Yeah...you’re right. #Sunny_Disheartened
    Sunny: Thank you Violet. #Sunny_Neutral
- "Beginner":
    // Race2Lose
    Sunny: We lost… And I thought {GREMLIN_NAME} and I were getting better at understanding each other. #Sunny_Disheartened
    Violet: Hmm, there’s still probably stuff you don’t know about them, building a friendship with your Gremlin takes time. #Violet_Disheartened
    Sunny: I already thought we were pretty close… But it seems like I have a long way to go.
    Violet: Hey, don’t beat yourself up! You and {GREMLIN_NAME} had fun and you learned something. That’s not bad if you ask me. #Violet_BigSmile
    Violet: There’s always more races to be had, so next time just give it your all! #Violet_Talking
- "Intermediate":
    // Race3Lose
    Sunny: Oh… We lost. #Sunny_Disheartened
    Violet: Well, you and {GREMLIN_NAME} didn’t win, but I saw both of you give it your all out there. Good effort. #Violet_Disheartened
    Sunny: Thank you. I guess we just need to work a little harder. I thought we were doing really good, but there are so many better teams out there.
    Violet: Hey, just because you two aren’t the star racing duo now doesn’t mean you’ll never make it. #Violet_NeutralFrown
    Violet: At the end of the day, this is just one race of many! What matters above all is the friendship you and {GREMLIN_NAME} share. #Violet_Disheartened
    Violet: As long as you remember that, you and {GREMLIN_NAME} will be tearing up the track in no time! #Violet_Talking
    Sunny: That’s true… Thank you, Violet. #Sunny_Neutral
    Sunny: Alright, {GREMLIN_NAME}, let’s keep giving it our best! #Sunny_Determined
- "Expert":
    // Race4Lose
    Sunny: As expected of the big leagues, that was really intense! #Sunny_Shock
    Sunny: You feeling alright, {GREMLIN_NAME}? #Sunny_Shock
    Violet: I hear you. This race is rough. Even for Jellybeans. #Violet_Talking
    Sunny: Even though we didn’t win, I think I learned a lot from this race. #Sunny_Disheartened
    Sunny: I cheered my heart out, but I heard you and the others cheer really loud as well. #Sunny_Neutral
    Sunny: Everyone’s trying so hard… I just need to keep at it, and try even harder. #Sunny_Determined
    Violet: That’s an amazing mentality for a rookie. #Violet_Disheartened
    Violet: I can see it. You’re going to win big one day. #Violet_Talking
    Sunny: Aww, thank you. I know that you’re a winner, too, Violet. You’re definitely going to win all of your races! #Sunny_BigSmile
    Violet: Well, there’s only one way to find out which of us is right. #Violet_NeutralFrown
    Sunny: Race again! #Sunny_Determined
    Violet: Yeah! #Violet_BigSmile
- else:
    -> GenericPostRace
}
-> END

=== GenericPostRace ===
VAR chosenGenericScene = 1
~ chosenGenericScene = RANDOM(1, 3)
{ WON_RACE:
- 1:
    { chosenGenericScene:
    - 1:
        // GenericRaceWin1
        Sunny: Yes! We won! #Sunny_BigSmile
        Violet: Congrats, Sunny. You’re winning more and more now, how does it feel? #Violet_Talking
        Sunny: I could get hooked on this! It feels great! #Sunny_Determined
    - 2:
        // GenericRaceWin2
        Sunny: Great job out there, {GREMLIN_NAME}! We won! #Sunny_BigSmile
        Violet: Amazing. You sure have put in a lot of hard work. I can tell! #Violet_Talking
        Sunny: Thanks! You and Jellybeans were great out there as well. Let’s keep on giving it one hundred percent!
    - 3:
        // GenericRaceWin3
        Sunny: Nice work, {GREMLIN_NAME}! #Sunny_BigSmile
        Violet: Wow! You really one-upped me and Jellybeans this time, huh? #Violet_Talking
        Sunny: Yeah! Bring out the plates, because you just got served! #Sunny_Determined
        Sunny: Sorry. I just wanted to try something corny. #Sunny_BigSmile
        Violet: Haha, you’ve become a veteran Grom already. #Violet_BigSmile
    }
- 0:
    { chosenGenericScene:
    - 1:
        // GenericRaceLose1
        Sunny: Aww... We lost... #Sunny_Disheartened
        Violet: Well, you win some, you lose some. Cheer up, Sunny. #Violet_Disheartened
        Sunny: You’re right, I won’t let this get me down! #Sunny_Determined
    - 2:
        // GenericRaceLose2
        Sunny: Rats! We lost! We were so close, too! #Sunny_Shock
        Violet: I feel you. The closer you are to winning, the worse it feels when you lose. #Violet_Disheartened
        Sunny: Yeah… But being that close means our training is working! We’ll definitely do better in the next race! #Sunny_Determined
        Violet: That’s the Sunny I know! #Violet_BigSmile
    - 3:
        // GenericRaceLose3
        Sunny: Oh no! {GREMLIN_NAME} are you okay? #Sunny_Shock
        Violet: Well, that looked like quite the tumble. Reminds me of that time someone fell down a flight of stairs. #Violet_Disheartened
        Sunny: Oh no! Jellybeans did?
        Violet: No… It was... Me. #Violet_NeutralFrown
        Sunny: ???????????
    }
}
->END