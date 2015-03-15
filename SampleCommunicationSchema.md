# Game State #
```
<game>
   <status turn="1" />
   <message value="Updated" />
   <players>
      <player id=0 balance=400>
          <area>
             <CardPile id=0 open="false" expandable="true">
                <card id=12 status="faceup" moveable="false" />
             </CardPile>
             <CardPile id=2 open="false" expandable="true">
                <card id=16 status="faceup" moveable="false" />
             </CardPile>
             <ChipPile open="false">
                <chip guid="4jklrjklfu4jnkfu8943runf" value="50" color="blue" moveable="false"  />
                <chip guid="d34jhdf7f45jkljf894lkfhd" value="20" color="red" moveable="false" />
                <chip guid="jkl98dfjr4jnkf89fj3udjdl" value="10" color="green" moveable="false" />
             </ChipPile>
          </area>
          <hand>
             <CardPile playable="true" style="hand">
                <card id=4 status="hidden" moveable="false" />
                <card id=12 status="faceup" moveable="false" />
                <card id=40 status="facedown" moveable="false" />
             </CardPile>
          </hand>
          <commands>
             <command name="Hit" action="Hit" />
          </commands>
      </player>
   </players>
   <area>
      <ChipPile open="true">
         <chip guid="4jklrjklfu4jnkfu8943runf" value="50" color="blue" moveable="false" />
         <chip guid="d34jhdf7f45jkljf894lkfhd" value="20" color="red" moveable="false" />
         <chip guid="jkl98dfjr4jnkf89fj3udjdl" value="10" color="green" moveable="false" />
      </ChipPile>
      <CardPile id=0 playable="true" style="deck">
         <card id=4 status="hidden" moveable="false" />
         <card id=12 status="faceup" moveable="false" />
         <card id=40 status="facedown" moveable="false" />
      </CardPile>
      <CardPile id=1 playable="false" style="discard" />
   </area>
</game>
```

# Chips #
```
<ChipPile open="true">
   <chip guid="4jklrjklfu4jnkfu8943runf" value="50" color="blue" moveable="false" />
   <chip guid="d34jhdf7f45jkljf894lkfhd" value="20" color="red" moveable="false" />
   <chip guid="jkl98dfjr4jnkf89fj3udjdl" value="10" color="green" moveable="false" />
</ChipPile>
```

# Card Collection #

Playable: true or false if the pile of cards can be "edited".  The piles of cards that can be moved or modified.

Style: layout (displays cards for players to see), deck (where cards are drawn from), discard, hand
```
<CardPile playable="true" style="hand">
   <card id=4 status="hidden" moveable="false" />
   <card id=12 status="faceup" moveable="false" />
   <card id=40 status="facedown" moveable="false" />
</CardPile>
```

# Commands #
Commands sent from the table to the server
```
<action game="A_Game_GUID_Here">
   <command action="Custom">
      <param name="action" value="hit" />
      <param name="player" value="A_Player_GUID_Here" />
   </command>
<action>
```

The move command specifies how an object moves from one collection to another.
```
<action game="A_Game_GUID_Here">
   <command action="Move">
      <param name="Player" value="A_Player_GUID_Here" />
      <param name="Object" value="A_PhysicalObject_GUID_Here" />
      <param name="To" value="A_Pile_GUID_Here" />
   </command>
<action>
```