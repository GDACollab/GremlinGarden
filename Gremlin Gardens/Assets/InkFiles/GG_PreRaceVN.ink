VAR GREMLIN_NAME = "Default"
VAR SELECTED_RACE = "starter"
VAR SEEN_UNIQUE = 0

>>> UpdateInkVar(GREMLIN_NAME, gremlinToRace)
>>> UpdateInkVar(SELECTED_RACE, selectedRace)
>>> UpdateInkVar(SEEN_UNIQUE, seenUnique)

>>> TextboxEnter(Default);
>>> !CharEnter(Sunny); CharEnter(Violet)

{ SEEN_UNIQUE:
- 1:
    -> GenericPreRace
- 0:
    { SELECTED_RACE:
    - "Starter":
        // Race1Pre
        Violet: Hey! I haven’t seen you around before. Are you new here? #Violet_Talking
        Sunny: Yeah! I’m Sunny, and I’m here with my gremlin {GREMLIN_NAME}. 
        Sunny: This is gonna be my first real race. I’m pumped! #Sunny_Determined
        Violet: First real race, huh? That’s so cool! I wish I could experience my first race all over again, it was so exciting! #Violet_Neutral
        Violet: Oh! I forgot to introduce myself. I’m Violet, and this is my gremlin Jellybeans. #Violet_BigSmile
        Violet: Would you like some pointers? #Violet_Talking
        Sunny: Sure! #Sunny_Neutral
        Violet: Alright. Gremlin races are split into four sections- Running, Climbing, Swimming, and Flying. #Violet_Neutral
        Violet: Your Gremlin will know what to do in each section, so all you have to do is cheer them on.
        Violet: Cheering your gremlin on will help them push their limits and go even faster! #Violet_Talking
        Violet: At least, Jellybeans runs faster when I cheer. #Violet_BigSmile
        Sunny: All right! I’ll cheer my heart out. Let’s have fun in this race! #Sunny_BigSmile
    - "Beginner":
        // Race2Pre
        Sunny: Oh, it’s Violet! C’mon {GREMLIN_NAME}, let's go say hi! #Sunny_BigSmile
        Violet: Sunny! Glad to see you again! So, how are you feeling after your first real race? 
        Sunny: Great! I’ll admit I was really nervous, but once I saw how much fun {GREMLIN_NAME} was having, I couldn’t help but have fun too. #Sunny_Neutral
        Violet: Yeah! Our gremlins really give it their all in those races, so it’s important to support them in any way we can! #Violet_Talking
        Violet: Jellybeans and I have been racing together for a while now, but we had to work hard to build the bond we have. #Violet_Neutral
        Violet: We weren’t that close at first, but then we noticed both of us had a habit of working ourselves too hard. #Violet_Disheartened
        Violet: Somehow, we ended up bonding over that. #Violet_BigSmile
        Sunny: That’s amazing! I hope that I can have that kind of bond with {GREMLIN_NAME}. #Sunny_BigSmile
        Sunny: I’ll admit, I feel like I’m not being the best parent to my gremlins as I can. I feel like I’m lacking as a Grom. #Sunny_Disheartened
        Violet: If you love and care for your gremlin, there's no wrong way to be a Grom. #Violet_Neutral 
        Violet: C’mon! The race is about to start, and {GREMLIN_NAME} is counting on you to cheer them on! #Violet_Talking
        Sunny: Yeah! I won’t let them down! #Sunny_Determined
    - "Intermediate":
        // Race3Pre
        Violet: Ready for the next race Sunny? #Violet_Talking
        Sunny: Mhm! {GREMLIN_NAME} and I have been working hard together and I think it’ll pay off! How about you and Jellybeans? #Sunny_BigSmile
        Violet: Ready as we’ll ever be! We just got our daily run in, and we’ve been honing our flying skills! After this race I think we’ll do some long distance swimming practice! #Violet_Neutral
        Sunny: Oh wow, the both of you? #Sunny_Shock
        Violet: Hm? You don’t work out with your gremlin as well? #Violet_Talking
        Sunny: Well, other than going on some runs with {GREMLIN_NAME}, not really… #Sunny_Neutral
        Sunny: I didn’t know I had to train with my gremlins too. #Sunny_Shock
        Violet: That’s alright! Everybody has a different way of training with their gremlin. #Violet_BigSmile #Sunny_Neutral
        Violet: I only do it this way because I used to be a runner. I’ve even made this spreadsheet to track our progress. #Violet_Talking
        Violet: Jellybeans really likes these. He’ll sometimes stare at these for hours. #Violet_Disheartened
        Sunny: Huh… You really do share a deep bond with your gremlin. I’m a little jealous. #Sunny_Shock
        Sunny: But that won’t stop me and {GREMLIN_NAME} We’re going to give it our all! #Sunny_Determined
        Violet: Good! We like a little challenge! #Violet_BigSmile
    - "Expert":
        // Race4Pre
        Sunny: Alright… This is it, Sunny. The biggest race on the island. #Sunny_Determined
        Sunny: I wonder if {GREMLIN_NAME} is ready for this… #Sunny_Disheartened
        Violet: You okay, Sunny? You look nervous. #Violet_Disheartened
        Sunny: Oh! I didn’t notice you there. Yeah, I’m a little worried about this race… It’s just so big. #Sunny_Neutral
        Violet: Well, you should try not to fidget around so much. It might make {GREMLIN_NAME} worry about you. #Violet_NeutralFrown
        Sunny: {GREMLIN_NAME}, worry about me? #Sunny_Shock
        Violet: It happens. Jellybeans here is especially good at telling when I’m nervous. #Violet_Talking
        Violet: He’s actually pretty good at taking care of others; a bunch of gremlins follow him around all the time. #Violet_Neutral
        Violet: I think they voted him as their President or something. #Violet_Talking
        Sunny: Haha, I didn’t know you told jokes this funny, Violet. #Sunny_Neutral
        Violet: But I wasn’t-- #Violet_Talking
        Sunny: Oh! That’s the starting announcement. Gotta go! #Sunny_BigSmile
        >>> !CharExit(Sunny)
        Violet: I hope she’ll be alright… #Violet_NeutralFrown
    - else:
        -> GenericPreRace
    }
}

-> END

=== GenericPreRace ===
VAR chosenGenericScene = 1
~ chosenGenericScene = RANDOM(1, 3)
{ chosenGenericScene:
- 1:
    // GenericRacePre1
    Sunny: Hi, Violet! Nice day to race, huh? #Sunny_BigSmile
    Violet: Yep! A nice cool breeze like this makes me want to go for a run, too. #Violet_Talking
    Sunny: Yeah! It’s perfect for getting fired up. Let’s go, {GREMLIN_NAME}! #Sunny_Determined
- 2:
    // GenericRacePre2
    Sunny: Hi, Violet! Hi, Jellybeans! Great too see you again. #Sunny_BigSmile
    Violet: And you too, Sunny. How’s {GREMLIN_NAME} doing? #Violet_Talking
    Sunny: Great! Good enough to bring the fight to Jellybeans! #Sunny_Determined
    Violet: I’d rather you not, he has a lot on his plate already. #Violet_NeutralFrown
    Sunny: Oh, sorry. It’s just a saying. Anyways, let’s race! #Sunny_BigSmile
- 3:
    // GenericRacePre3
    Violet: Heya, Sunny. We’re running into each other a lot, huh? #Violet_BigSmile
    Sunny: Yeah. I’m starting to really like racing now. #Sunny_Determined
    Violet: I can tell. I don’t really know any other trainer that has as many gremlins racing as you do. #Violet_Neutral
    Violet: Why do you race with so many gremlins, anyways? #Violet_NeutralFrown
    Sunny: Because my dream is to be #1 Grom! … And they’re all precious. #Sunny_Neutral
    Violet: Well, good luck. Both on your dream, and in this race. #Violet_Talking
    Sunny: You too! #Sunny_BigSmile
}
->END