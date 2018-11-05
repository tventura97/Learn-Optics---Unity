# Learn-Optics---Unity
(Use the develop branch. This branch is behind.)


Optics Application for NJIT REU
Educational Android app designed to supplement a university-level physics curriculum. Graphically demonstrates and teaches ray-tracing concepts.
There's a bunch of technical stuff so if you just wanna see how the app works/looks, you can skip down to the bottom. 
Currently only designed for 16:9 aspect ratios. The UI elements will not be rendered properly on a screen with a longer than normal display (say like the Pixel 2 XL at 18:9). Will implement a fix after midterm season.

## Introduction

The program teaches users the fundamentals of geometric [ray-tracing](http://hyperphysics.phy-astr.gsu.edu/hbase/geoopt/raydiag.html).
It creates a sandbox environment that allows users to play with different types of optical elements (convex/concave lenses, mirrors, etc) so that they can learn about ray-tracing in a more responsive and interactive way.

## How the app works
The application calculates the trajectory of the light rays using [ray transfer matrix analysis](https://en.wikipedia.org/wiki/Ray_transfer_matrix_analysis). It uses matrices to describe an optical element's effect on a beam of light, which simplifies the calculations. Basically, each optical element (mirrors, lenses) gets a property called a transfer matrix. When a beam of light collides with the element, the new trajectory of the light ray is calculated with using the transfer matrix. 

![ray-transfer-matrix-analysis](https://i.imgur.com/pOSKAUS.png)

To define the transfer matrix for each optical element, one could simply predefine each
matrix for each optical element programmatically. However, this is a fairly difficult method of
implementation as the properties of each optical element had to be dynamically modifiable by the
user at runtime. To address this issue, the optical elements were modelled in Blender and given a mesh collider in Unity so that
they can interact with the light rays. Due to the paraxial ray assumption made in the majority of geometric optics, we can simplify every calculation by assuming that every collision a light ray makes with an optical element is identical to the light ray colliding with a very small plane. Taking advantage of this assumption, every optical element was defined as a large number of small faces combined to form the desired shapes of each element. Unity is able to extract all of the necessary information required for the determination of the transfer matrix from the collision. Thus, transfer matrices are calculated dynamically rather than pre-defined. 

### Optical Element Modeled in Blender

![3d-element-blender](https://i.imgur.com/kJ7sBRC.png)


### App screenshots (explanations, videos & demos coming soon)
![app-menu](https://i.imgur.com/oaO0eWT.png)

![sandbox-mode](https://i.imgur.com/EhYGrBF.png)

![analysis-of-formed-image](https://i.imgur.com/xjPsbzs.png)

![equation-panel](https://i.imgur.com/kYsQ1t7.png)

![single-frame-from-tutorial](https://i.imgur.com/2BnMg0h.png)
