# TweetingMachines language specification

## Percussion

### Hit types
	1 → percussion 1
	2 → percussion 2
	3 → percussion 3
	4 → percussion 4

### Single hit

	1

### Single hit, repeated
	1 1 1 1

### Playing simultaneous hits
	12

### Simultaneous hits, repeated
	12 12 12 12

## Timing adjustments 

### Before the beat (1/8th)
	1 1 1 \1

### After the beat (1/8th)
	1 1 1 /1

### 2/8ths early
	1 1 1 2\1
or

	1 1 1 \\1

### 4/8ths late
	1 1 1 4/1
or

	1 1 1 ////1

### Before the beat with the following hit on the beat
	1 1 \1 1

### Skip a beat
	1 1 . 1 1
