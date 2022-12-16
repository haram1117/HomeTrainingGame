# Health Battle on the Lake
[2022 Fall] Capstone Design Project 

[최종 보고서.pdf](https://github.com/haram1117/HomeTrainingGame/files/10243091/default.pdf)

### Summary

XR기반 홈트레이닝 시스템 및 근전도 센서를 활용한 게이미피케이션

전세계가 함께 팬데믹 사태를 겪으며 각자의 집에서 보내는 시간이 굉장히 많아졌다. 그에 따라 자연스럽게 집에서 자신의 건강을 위해 운동하는 홈 트레이닝이 주목받고 있다. 혼자서 하는 경우가 많은 홈트레이닝 특성상 자신의 자세가 옳은 자세인지 피드백을 받기는 쉽지 않고, 또한 지루하기도 하다. 이에 근전도 센서를 이용해 각각의 자세에 따라 어느정도 자극을 근육이 느끼는지 측정하고, 해당 근육의 자극정도를 통해 정자세로 운동을 수행하는지 피드백을 주고자 한다. 이때, XR을 이용한 게임적 요소를 결합하여 이용자들이 더 자연스럽게 더 높은강도로 또 정자세로 운동을 할 수 있도록 하는 Controller와 소프트웨어를 구현하고, 이를 통해 상세한 게임적 요소와 피드백이 우리의 운동강도에 미치는 영향을 연구해 보고자 한다.

### Table of Contents
1. [Description](#description)
    - [1. Overviews](#1-overviews)
    - [2. Story](#2-story)
    - [3. Tech](#3-tech)
2. [How To Play](#how-to-play)
3. [Team Members](#team-members)

## Description

### 1. Overviews
![GameTitle](https://user-images.githubusercontent.com/63827499/206608848-2ce229ae-108b-45df-850c-451d91ea96f1.png)

#### Title
Health Battle on the Lake

#### Theme
게임 속 캐릭터가 되어 게임 속 친구와 집에서 운동하며 스타를 모아 승리한다.

#### Platform
Microsoft Windows, MacOS

#### Genre
체감형 캐주얼 게임

#### Target
홈 트레이닝에 관심있는 전 연령
<br/><br>
### 2. Story
#### 1. Main  Character
|![boy](https://user-images.githubusercontent.com/63827499/206609997-b671e45e-48d4-45fe-999f-e6e1cb17e1f5.png)|![girl](https://user-images.githubusercontent.com/63827499/206609998-ec7a16ed-a678-4b43-8205-40b3a0f20add.png)|
|:---:|:---:|
|Leonard (Boy)|Haley (Girl)|
#### 2. Time and Place
- 시간: 1980년 - 2000년 초반
- 장소: 호수를 둘러싸고 있는 마을
#### 3. Story TimeLine
- 1980.09.24
  - 쌍둥이 Leonard & Haley 탄생
- 1991.12.03
  - 마을에 큰 폭동 발생
  - Leonard 납치
- 2001.04.01
  - 10년간 준비한 Haley의 복수의 날
  - 폭동족들과 함께 행복한 삶을 살고 있는 Leonard에게 배신감을 느끼고 결투 신청
  
호수를 둘러싸고 있는 마을을 배경으로 Haley와 Leonard가 결투를 벌인다.
<br/></br>
### 3. Tech
#### 1. Platform
![platform](https://user-images.githubusercontent.com/63827499/206659291-cdd360cd-5f77-4bae-b927-eeb501949a01.png)
- Barraduda 에서 제공하는 Supported Platforms

- Windows, MacOS 기반 웹캠과 GPU가 있는 노트북 혹은 데스크탑을 타겟으로 한다.
  - NN기반 pose estimation을 실시간으로 수행하는 기능이 필요하기에 GPU가 필요함.
  
#### 2. Used Tools
- Engine: Unity Engine
- Programming Language: C#, Python
- Library: Barracuda, Photon PUN, Mediapipe 

#### 3. Project Architecture
###### 전반적인 프로젝트 아키텍처
![architecture](https://user-images.githubusercontent.com/63827499/206662876-bbd9c988-0ecb-47c3-8b83-975ac115b72e.png)

제스처 기반 사용자 입력을 처리하기 위한 Mirror View 카메라와 pose Estimation을 위한 탑뷰 카메라로 사용자를 촬영합니다. 탑뷰 카메라로 부터 받은 input source는 unity내에서 실행되는 Resnet 기반 동작 인식 모듈인 Barracua로 전송됩니다.

###### 미니게임에서의 플레이어 소프트웨어 구성
![asdf](https://user-images.githubusercontent.com/63827499/206663122-2071f966-1d7d-45df-98b2-e2fd345ec200.png)


## How To Play
- Unity Version: 2020.3.41f1
###### 1. Install Git LFS
- https://git-lfs.github.com/
```
git lfs install
```
###### 2. Download Our Game
```
git clone https://github.com/haram1117/HomeTrainingGame.git
```
###### 3. Download Hand Gesture Recognition Software
- https://github.com/haram1117/hand-gesture-recognition-mediapipe
- Check Requirements
```
https://github.com/haram1117/hand-gesture-recognition-mediapipe.git
```
###### 4. Open the Projects & Ready to Play
- Open the Unity Project
- Need 2 Cameras
  - 0: Pose Estimation
  - 1: Hand Gesture Recognition

###### 5. Run Hand Gesture Recognition
```
python app.py
```
###### 6. Play Game !


## Team Members
|이름|학번|이메일|
|:---:|:---:|:---:|
|김승찬|2018102174|seungchan1219@gmail.com|
|노현욱|2017103983|rohsik2@gmail.com|
|박하람|2019102181|haram1117@khu.ac.kr|
