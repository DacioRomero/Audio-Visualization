# Audio-Visualization

A three-mode audio/music visualizer with an oscilloscope initially created to imitate [this video](https://youtu.be/82Q6DRqf9H4), but ended up being much more.

## Visualizer (.unity)

This visualizer's sampling ranges are equal temper scaled, meaning they scale to how humans hear frequency, based off of Michigan Tech's notes on the [physics of music](https://pages.mtu.edu/~suits/Physicsofmusic.html), specifically the equation they provided on [this page](https://pages.mtu.edu/~suits/NoteFreqCalcs.html).

### To-Do

* Double check note scaling
* Verify decibel addition is done properly despite https://github.com/DacioRomero/Audio-Visualization/commit/6eb8645cc402baf64bff938e819b5724ec1b3ea3 saying it is
* Split code into functions
* Document
* Optimize

## Oscilloscope (.unity)

The oscilloscope was intended to simulate an oscilloscope in order to make mushrooms like [this video](https://youtu.be/rtR63-ecUNo). The values are currently randomly chosen, but it effectively imitates the video. 

### To-Do

* Better simulate an oscilloscope
	* Make areas with lots of overlap/slow movement go closer to white
* Document
* Optimize
