# Milestones

## 2020

Week of 2020-12-21
1. camera follow
2. monsters design (start with 2 types, design first, controlled movement, some types move in groups)
3. scene transitions
3. monster generator, has to be after design 

4. 卡在data representation上了，再来一遍

4. road game generation
    - noise for distribution

5. road random generation
    - Bezier curve

6. Clean script design!

6. can there be singleton GameObject without ? Generate GameObject from non-gameobject script (current solution: use singleton)(can add generic class to Generatable, and add a factory class)?


I decided that I will focus on the world generation, particularly the road generation for now. After this big goal is achived, I will then tackle the monsters' design and group genertaion. 

--- Recovery of Eczema ---

## 2021

Week of 2021-03-01
- F monsters going around the world
    5 types:
- F cannot leave road / road tracking design [x]
    (handled) illed cases
    -> dead corner (infinite loop)
    (unhandled) illed cases
    -> AOB < 90, 
    -> consecutive repeated points (should be able to handle it in the Player's end; also a result of the end of ) // ANCHOR next version update
- F better movement feeling (only moves camera when at the edge of the camera view)
- number design; no-end design. when trapped, can just wait in the snow until it gets better. "it will get better..." "let time heal..."
    modes of choosing: time-travel mode / real advanture mode (where no fast forward is allowed. slow gaming. (recommends good literature or movies while trapped.) )
- A trails of walken path left behind

- add eye blinking -> monsters state change (move faster /// later stage: be friendly)
    - Event & Notification system?

## // ANCHOR next version
- refactor road code into the Builder pattern, aka. Road Create functions will be put into the RoadGenerator class, and RoadGenerator will have a RoadGenerator interface. In the future can just extend the RoadGenerator interface with different code. NavBlock calc code also into the RoadGenerator class.
- more generally applicable data representation for the roads,  upgrade, when the , at the vertices (+ edges) instead of at the edges, undirectional graph;
    * candidate: uncyclic graph, check the article
    * also capable of handle all illed cases
        (handled) illed cases
        -> dead corner (infinite loop)
        (unhandled) illed cases
        -> AOB < 90,
        -> consecutive repeated points (should be able to handle it in the Player's end; also a result of the end of ) // ANCHOR next version update
    <!-- * ~~performance boost~~ -->



## Goals
Art game / experience game / symbolic narrative game
- simple (maybe classic) mechanics that brings some fun


Designing or Describing
- Speedgraphing. (core design value)


## Non-goals
- snow accumulating while the Eye is open for longer. Environmental change and interaction.
- Stages: I. Realization : realizing the Eye's existance, II. battling with the Eye, III. realizing the Eye cannot die, can always reborns; VI. find more sustainable way to co-exist with it.



## References

1. https://en.wikipedia.org/wiki/Point_in_polygon
