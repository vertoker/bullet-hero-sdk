# Afterbeat's own themes

Specification for the themes Afterbeat ships with (sorry, it's for interop, not for stealing).

**Why this document exists.** A level that never authored a palette of its own carries an EMPTY
`themes[]` and names one of these by INDEX - the theme track holds the string `"19"` and nothing
else. Read against the file alone that is a dangling reference, so such a level used to import with
no theme at all and every theme-referenced colour resolved to white. `Maps/ABDefaultThemes`
is the table that fixes it; this document is the same data in a readable form.

**The index IS the id**, `"0"` through `"20"`, in the order below - that is what a `.vgd` stores
and what the importer looks up.

**Provenance.** Measured off the shipped game (`DataManager.BeatmapThemes`, serialized into
`Afterbeat_Data/level2`), not transcribed from the wiki. An earlier hand-written version of this
file covered only the last eleven themes and had two colours wrong - Vicious Goop's fourth parallax
colour, and HotPanda's tail. Re-measure rather than hand-edit if the game ships new ones.

**Slot counts.** Players is always 4 and the Tail is a single colour of its own (the `.vgt`
`base_gui_accent` field, which serves both the GUI accent and the player's tail). Parallax and
Effects are always 9. **Objects VARIES** - as few as 2 - and the older themes really do hold fewer
than nine; the slots past the end are not stored, here or in the source game.


### 0 - Machine

```
GUI        (1) - #212121
Background (1) - #94D8DB
Players    (4) - #E57373 #64B5F6 #81C784 #FFB74D
Tail       (1) - #EF5350
Objects    (5) - #C0ACE1 #F17BB8 #2F426F #1B1B1C #EFEBEF
Parallax   (9) - #111111 #111111 #111111 #111111 #111111 #111111 #111111 #111111 #111111
Effects    (9) - #FF7C96 #EF4768 #B7003E #6D6E99 #41436B #161C40 #66E0FF #00AEEF #007FBC
```

### 1 - Anarchy

```
GUI        (1) - #212121
Background (1) - #FFFFFF
Players    (4) - #E57373 #64B5F6 #81C784 #FFB74D
Tail       (1) - #EF5350
Objects    (8) - #FFE7E7 #C0ACE1 #F17BB8 #2F426D #4076DF #6CCBCF #1B1B1C #EFEBEF
Parallax   (9) - #111111 #111111 #111111 #111111 #111111 #111111 #111111 #111111 #111111
Effects    (9) - #111111 #111111 #111111 #111111 #111111 #111111 #111111 #111111 #111111
```

### 2 - Day Night

```
GUI        (1) - #212121
Background (1) - #FFFFFF
Players    (4) - #F44336 #2196F3 #4CAF50 #FF9800
Tail       (1) - #EF5350
Objects    (6) - #132036 #3E5376 #A5DCE9 #EF8E46 #FFCD86 #FFF5D5
Parallax   (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
Effects    (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
```

### 3 - Donuts

```
GUI        (1) - #212121
Background (1) - #FFFFFF
Players    (4) - #F44336 #2196F3 #4CAF50 #FF9800
Tail       (1) - #EF5350
Objects    (9) - #FF86A3 #FF6386 #FFDF91 #FFBF7A #8CEEFF #20DAF5 #EC9D75 #CE6852 #1B1B1C
Parallax   (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
Effects    (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
```

### 4 - Classic

```
GUI        (1) - #212121
Background (1) - #FFFFFF
Players    (4) - #F44336 #2196F3 #4CAF50 #FF9800
Tail       (1) - #EF5350
Objects    (6) - #346188 #517A9E #FF6D8B #C2415F #1B1B1C #EFEBEF
Parallax   (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
Effects    (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
```

### 5 - New

```
GUI        (1) - #212121
Background (1) - #FFFFFF
Players    (4) - #F44336 #2196F3 #4CAF50 #FF9800
Tail       (1) - #EF5350
Objects    (8) - #1395BA #0D3C55 #C02E1D #F16C20 #EBC844 #A2B86C #1B1B1C #EFEBEF
Parallax   (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
Effects    (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
```

### 6 - Dark

```
GUI        (1) - #EFEBEF
Background (1) - #030436
Players    (4) - #F44336 #2196F3 #4CAF50 #FF9800
Tail       (1) - #EF5350
Objects    (5) - #20E4ED #71DF4F #F55A75 #FEFEFE #1B1B1C
Parallax   (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
Effects    (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
```

### 7 - Black/White

```
GUI        (1) - #EFEBEF
Background (1) - #111111
Players    (4) - #F44336 #2196F3 #4CAF50 #FF9800
Tail       (1) - #EF5350
Objects    (2) - #FAFAFA #FFFFFF
Parallax   (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
Effects    (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
```

### 8 - White/Black

```
GUI        (1) - #212121
Background (1) - #FAFAFA
Players    (4) - #F44336 #2196F3 #4CAF50 #FF9800
Tail       (1) - #EF5350
Objects    (3) - #222222 #333333 #444444
Parallax   (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
Effects    (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
```

### 9 - Poison

```
GUI        (1) - #F7F7F7
Background (1) - #3A3A58
Players    (4) - #F44336 #2196F3 #4CAF50 #FF9800
Tail       (1) - #EF5350
Objects    (3) - #66E0FF #FF7C96 #6D6E99
Parallax   (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
Effects    (9) - #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7 #F7F7F7
```

### 10 - Desert Heat

```
GUI        (1) - #111111
Background (1) - #FCE8C7
Players    (4) - #FA5C66 #5C8BFA #06D6A0 #FFD166
Tail       (1) - #EF5350
Objects    (9) - #F7B846 #FF6B45 #ED5052 #ED4F5D #7F492F #68392F #41231F #301B1A #FFFFFF
Parallax   (9) - #FDE9C6 #ECD8B7 #DDCAAC #C2AC94 #A48E79 #877163 #FDD592 #FEC09A #F9B8A6
Effects    (9) - #FF7C96 #EF4768 #B7003E #6D6E99 #41436B #161C40 #66E0FF #00AEEF #007FBC
```

### 11 - Ember Stones

```
GUI        (1) - #F7F7F7
Background (1) - #161A19
Players    (4) - #FA5C66 #5C8BFA #06D6A0 #FFD166
Tail       (1) - #EF5350
Objects    (9) - #FEDC85 #EFAA67 #AA674A #2C3A3B #404642 #5B5E55 #6B6855 #A49774 #161A19
Parallax   (9) - #161A19 #0C100F #060A09 #2D342D #212723 #1C211D #3D3D33 #292C25 #212320
Effects    (9) - #FF7C96 #EF4768 #B7003E #6D6E99 #41436B #161C40 #66E0FF #00AEEF #007FBC
```

### 12 - FireArmour

```
GUI        (1) - #111111
Background (1) - #F6A410
Players    (4) - #FA5C66 #5C8BFA #06D6A0 #FFD166
Tail       (1) - #EF5350
Objects    (9) - #15262A #375C7E #539DAE #EED32F #EBC22B #D6510F #CA271C #AB1F18 #6C100F
Parallax   (9) - #F6A410 #F56D04 #FB4800 #FE1F02 #F40400 #D70103 #C10908 #921010 #6D1916
Effects    (9) - #FF7C96 #EF4768 #B7003E #6D6E99 #41436B #161C40 #66E0FF #00AEEF #007FBC
```

### 13 - Jungle Waterway

```
GUI        (1) - #111111
Background (1) - #F2FAAC
Players    (4) - #FA5C66 #5C8BFA #06D6A0 #FFD166
Tail       (1) - #EF5350
Objects    (9) - #BBD52F #8FA42C #455306 #2E3C00 #2A2B17 #6A864C #3C5329 #000000 #FFFFFF
Parallax   (9) - #F2FAAC #FBFDCF #F7FCBE #EFFD8E #E4F16F #D9EA6A #E2F1A0 #D8E998 #C8D989
Effects    (9) - #FF7C96 #EF4768 #B7003E #6D6E99 #41436B #161C40 #66E0FF #00AEEF #007FBC
```

### 14 - Lure

```
GUI        (1) - #F7F7F7
Background (1) - #0A1D2B
Players    (4) - #FA5C66 #5C8BFA #06D6A0 #FFD166
Tail       (1) - #EF5350
Objects    (9) - #21F1FD #0CAFFC #0575FB #0943C8 #183C92 #FFFD8C #F3D457 #DDA73D #AF7F2D
Parallax   (9) - #0A1D2B #132941 #142A4A #0F2F48 #143D5D #131420 #262837 #3C3A4F #3F4662
Effects    (9) - #FF7C96 #EF4768 #B7003E #6D6E99 #41436B #161C40 #66E0FF #00AEEF #007FBC
```

### 15 - Shiver

```
GUI        (1) - #323233
Background (1) - #BCDFF8
Players    (4) - #FA5C66 #5C8BFA #06D6A0 #FFD166
Tail       (1) - #EF5350
Objects    (9) - #99FFFF #52E1FB #52ADF7 #407DDD #0635FF #0419CB #3D7F83 #276563 #112437
Parallax   (9) - #BCDFF8 #C6ECFB #DAF7FD #86B2EC #7195C0 #7D97B3 #5E7B98 #495E78 #32485F
Effects    (9) - #FF7C96 #EF4768 #B7003E #6D6E99 #41436B #161C40 #66E0FF #00AEEF #007FBC
```

### 16 - Starlight

```
GUI        (1) - #F2F2F2
Background (1) - #05132D
Players    (4) - #FA5C66 #5C8BFA #06D6A0 #FFD166
Tail       (1) - #EF5350
Objects    (9) - #246B67 #3C9885 #70C09C #7ACA90 #D4EC9B #F5FC7F #AAE2F1 #FFFFFF #05132D
Parallax   (9) - #05132D #082336 #072A3E #0F384A #184D58 #010E17 #19533B #003E2E #16403C
Effects    (9) - #FF7C96 #EF4768 #B7003E #6D6E99 #41436B #161C40 #66E0FF #00AEEF #007FBC
```

### 17 - Stone Field

```
GUI        (1) - #F2F2F2
Background (1) - #28323B
Players    (4) - #FA5C66 #5C8BFA #06D6A0 #FFD166
Tail       (1) - #EF5350
Objects    (9) - #A36D4B #7A5640 #AF9D90 #C2A599 #DCC2BD #B99798 #7D5750 #603F3C #402425
Parallax   (9) - #28323B #313D48 #354650 #3E5865 #637583 #77838F #262D33 #1E272C #181F25
Effects    (9) - #FF7C96 #EF4768 #B7003E #6D6E99 #41436B #161C40 #66E0FF #00AEEF #007FBC
```

### 18 - Vicious Goop

```
GUI        (1) - #323233
Background (1) - #B3CBB4
Players    (4) - #FA5C66 #5C8BFA #06D6A0 #FFD166
Tail       (1) - #EF5350
Objects    (9) - #EC412F #FE674E #FEAC7C #FCD9B3 #B7A691 #A58B76 #83695B #000000 #FFFFFF
Parallax   (9) - #B3CBB4 #ACBFA8 #A4B29B #7A8F81 #96AC9A #A5BCA7 #5E8C87 #88AB9D #9DBAA8
Effects    (9) - #FF7C96 #EF4768 #B7003E #6D6E99 #41436B #161C40 #66E0FF #00AEEF #007FBC
```

### 19 - Wonderland

```
GUI        (1) - #323233
Background (1) - #FFAAFF
Players    (4) - #FA5C66 #5C8BFA #06D6A0 #FFD166
Tail       (1) - #EF5350
Objects    (9) - #FD2FAB #961865 #410930 #240419 #8151AF #C860CE #FF7DF8 #FFAAFF #FFFFFF
Parallax   (9) - #FFAAFF #FEB8FE #FAC4FE #FF8BFE #FF97FE #FEA3FF #B56BAE #D98AD6 #ED9AEB
Effects    (9) - #FF7C96 #EF4768 #B7003E #6D6E99 #41436B #161C40 #66E0FF #00AEEF #007FBC
```

### 20 - HotPanda

```
GUI        (1) - #FFFFFF
Background (1) - #27232A
Players    (4) - #FA5C66 #5C8BFA #06D6A0 #FFD166
Tail       (1) - #C80224
Objects    (9) - #C8024F #FF585F #FCE4A8 #BED7E9 #0A7E8B #093A54 #2F2041 #27232A #FFFFFF
Parallax   (9) - #FFAAFF #FEB8FE #FAC4FE #FF8BFE #FF97FE #FEA3FF #B56BAE #D98AD6 #ED9AEB
Effects    (9) - #FF7C96 #EF4768 #B7003E #6D6E99 #41436B #161C40 #66E0FF #00AEEF #007FBC
```
