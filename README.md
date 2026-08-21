# KEMET

Unity로 개발한 2D 플랫포머 액션 게임입니다.

플레이어의 이동, 점프, 슬라이딩, 콤보 공격과 패링을 구현하고, State Pattern과 FSM을 기반으로 다양한 몬스터 및 보스의 행동과 전투 패턴을 구성한 개인 프로젝트입니다.

---

## 목차

- [프로젝트 정보](#프로젝트-정보)
- [주요 구현 기능](#주요-구현-기능)
- [플레이어 시스템](#플레이어-시스템)
- [몬스터 AI](#몬스터-ai)
- [FSM 구조](#fsm-구조)
- [몬스터별 행동 구조](#몬스터별-행동-구조)
- [프로젝트 구조](#프로젝트-구조)
- [실행 환경](#실행-환경)
- [플레이 영상](#플레이-영상)

---

## 프로젝트 정보

| 항목 | 내용 |
| --- | --- |
| 프로젝트명 | KEMET |
| 플랫폼 | PC |
| 장르 | 2D 플랫포머 액션 |
| 개발 기간 | 2023.02 ~ 2025.02 |
| 개발 인원 | 1인 개발 |
| 게임 엔진 | Unity 2022.3.40f1 |
| 개발 언어 | C# |
| 개발 도구 | Unity, Visual Studio, GitHub |

---

## 주요 구현 기능

### 플레이어

- 좌우 이동
- 점프
- 슬라이딩
- 일반 공격
- 콤보 공격
- 패링
- 피격 및 사망
- 애니메이션과 전투 로직 연동

### 몬스터

- 플레이어 탐지
- 순찰
- 추적
- 공격
- 돌진
- 소환
- 그로기
- 피격 및 사망
- 몬스터 종류별 개별 FSM

### 게임 시스템

- Unit 기반 플레이어·몬스터 공통 구조
- State Pattern과 FSM 기반 AI
- 씬 이동 및 스테이지 전환
- 카메라 이동
- 프리팹 기반 캐릭터와 오브젝트 관리
- 애니메이션 및 이펙트 연동

---

## 플레이어 시스템

플레이어 캐릭터는 `Seth` 클래스를 중심으로 구성되어 있습니다.

공통 캐릭터 기능은 `Unit` 클래스에서 관리하며 플레이어는 이를 기반으로 이동과 전투 기능을 확장합니다.

### 이동

- 입력 방향에 따른 좌우 이동
- 캐릭터 방향 전환
- 지면 판정
- 점프
- 슬라이딩
- 상태에 따른 이동 제한

### 전투

- 공격 입력 처리
- 연속 공격 및 콤보 처리
- 공격 애니메이션 재생
- 공격 판정 활성화
- 피격 및 체력 감소
- 패링 성공 여부 판단
- 사망 처리

플레이어 입력, 애니메이션 및 공격 판정을 연계하여 공격 모션의 적절한 시점에 실제 전투 판정이 발생하도록 구성했습니다.

### 패링

플레이어가 적의 공격 타이밍에 맞춰 패링을 사용하면 일반 피격과 다른 결과를 적용하도록 구성했습니다.

패링 성공 여부에 따라 플레이어와 몬스터의 상태를 변경하고, 적을 그로기 상태로 전환할 수 있도록 전투 흐름을 구성했습니다.

---

## Unit 기반 공통 구조

플레이어와 몬스터가 공유하는 기본 속성과 동작은 `Unit` 클래스에서 관리합니다.

```text
Unit
 ├─ Player
 │   └─ Seth
 │
 └─ Monster
     ├─ PatrolMonster
     ├─ RushMonster
     ├─ NecromancerMonster
     └─ SummonerMonster
```

공통 기능을 상위 클래스에 배치하고 플레이어와 몬스터별 동작을 하위 클래스에서 확장하여 중복 코드를 줄이도록 구성했습니다.

---

## 몬스터 AI

몬스터의 행동을 하나의 클래스에서 조건문으로 관리하지 않고 상태별 클래스로 분리했습니다.

각 상태는 자신의 진입, 실행 및 종료 동작을 담당하고 AI 클래스는 현재 상태를 관리합니다.

```text
Monster
  ↓
Monster AI
  ↓
State Machine
  ↓
Current State
```

몬스터 종류마다 필요한 상태를 별도로 구성하여 행동 패턴을 독립적으로 확장할 수 있도록 구현했습니다.

---

## FSM 구조

### 상태 전환 흐름

```text
Idle
  ↓
Patrol
  ↓
Detect Player
  ↓
Chasing
  ↓
Attack
  ↓
Groggy / Dead
```

각 몬스터는 현재 상태에서 플레이어와의 거리, 체력, 공격 가능 여부 등의 조건을 검사해 다음 상태로 전환합니다.

### State Pattern 적용 목적

기존 방식처럼 하나의 Update 함수에서 모든 행동을 조건문으로 처리하면 몬스터 종류와 상태가 증가할수록 코드가 복잡해집니다.

상태별 행동을 클래스로 분리하여 다음과 같은 구조를 만들었습니다.

- 상태별 책임 분리
- 상태 전환 조건 명확화
- 몬스터별 행동 확장
- 새로운 상태 추가 시 기존 코드 수정 최소화
- 공격과 이동 로직의 독립적 관리

---

## 몬스터별 행동 구조

### 1. Patrol Monster

일정 구역을 순찰하다가 플레이어를 발견하면 추적하고 공격하는 기본 몬스터입니다.

```text
PMIdleState
  ↓
PMPatrolState
  ↓
PMChasingState
  ↓
PMAttackState
  ↓
PMGroggyState / PMDeadState
```

| 상태 | 역할 |
| --- | --- |
| `PMIdleState` | 대기 및 주변 상태 확인 |
| `PMPatrolState` | 지정된 구역 순찰 |
| `PMChasingState` | 플레이어 추적 |
| `PMAttackState` | 공격 실행 |
| `PMGroggyState` | 그로기 처리 |
| `PMDeadState` | 사망 처리 |

### 2. Rush Monster

플레이어를 발견하면 돌진 준비 후 빠르게 이동하여 공격하는 몬스터입니다.

```text
RMIdleState
  ↓
RMPatrolState
  ↓
RMStartRushingState
  ↓
RMRushingState
  ↓
RMStopRushingState
```

| 상태 | 역할 |
| --- | --- |
| `RMIdleState` | 대기 및 탐지 |
| `RMPatrolState` | 순찰 |
| `RMStartRushingState` | 돌진 방향과 준비 동작 처리 |
| `RMRushingState` | 플레이어 방향으로 돌진 |
| `RMStopRushingState` | 돌진 종료 및 상태 복구 |
| `RMGroggyState` | 그로기 처리 |
| `RMDeadState` | 사망 처리 |

돌진 준비, 실행 및 종료를 서로 다른 상태로 분리하여 돌진 공격의 각 단계를 독립적으로 관리했습니다.

### 3. Summoner Monster

플레이어와 직접 근접 전투를 하기보다 소환 몬스터를 생성하여 공격하는 몬스터입니다.

```text
SMIdleState
  ↓
SMPatrolState
  ↓
SMSummonState
  ↓
SMGroggyState / SMDeadState
```

| 상태 | 역할 |
| --- | --- |
| `SMIdleState` | 대기 및 플레이어 탐지 |
| `SMPatrolState` | 순찰 |
| `SMSummonState` | 소환 몬스터 생성 |
| `SMGroggyState` | 그로기 처리 |
| `SMDeadState` | 사망 처리 |

### 4. Summoned Monster

소환 몬스터는 소환자와 별도의 FSM을 사용합니다.

```text
SMDChasingState
  ↓
SMDStickState
  ↓
SMDDeadState
```

| 상태 | 역할 |
| --- | --- |
| `SMDChasingState` | 플레이어 추적 |
| `SMDStickState` | 플레이어에게 접근한 뒤 고유 행동 실행 |
| `SMDDeadState` | 사망 및 제거 |

소환자와 소환 몬스터의 상태를 분리하여 각각 독립적인 행동 흐름을 갖도록 구성했습니다.

### 5. Necromancer Monster

마법 기반 전투를 수행하는 몬스터로 별도의 AI 및 상태 구조를 통해 행동을 관리합니다.

다른 몬스터와 공통 FSM 구조를 사용하되 몬스터 특성에 맞는 공격 및 상태 전환을 독립적으로 구성했습니다.

---

## FSM 설계 효과

### 상태별 책임 분리

순찰, 추적, 공격, 그로기 및 사망 로직을 각각의 상태 클래스에서 처리하도록 분리했습니다.

### 몬스터별 확장

공통적인 상태 흐름을 유지하면서도 돌진과 소환처럼 몬스터 고유 행동을 별도의 상태로 추가할 수 있도록 구성했습니다.

### 상태 전환 가독성

현재 몬스터가 어떤 행동을 수행 중인지 상태 객체를 통해 명확하게 확인할 수 있도록 했습니다.

### 유지보수성

특정 몬스터의 공격 패턴을 수정할 때 다른 몬스터의 로직에 영향을 주지 않도록 클래스 구조를 분리했습니다.

---

## 씬 이동

`SceneMoveDoor`를 통해 특정 오브젝트와 상호작용하거나 조건을 만족했을 때 다음 씬으로 이동할 수 있도록 구성했습니다.

씬 단위로 스테이지를 분리하고 게임 진행에 따라 다음 구역으로 전환하도록 구현했습니다.

---

## 프로젝트 구조

```text
Assets/
├─ Scripts/
│  ├─ Core/
│  │  └─ Unit/
│  │     ├─ Unit.cs
│  │     │
│  │     ├─ Player/
│  │     │  └─ Seth.cs
│  │     │
│  │     └─ Monster/
│  │        ├─ FSM/
│  │        ├─ Monster.cs
│  │        │
│  │        ├─ PatrolMonster/
│  │        │  ├─ PatrolMonster.cs
│  │        │  ├─ PatrolMonsterAI.cs
│  │        │  └─ State/
│  │        │
│  │        ├─ RushMonster/
│  │        │  ├─ RushMonster.cs
│  │        │  ├─ RushMonsterAI.cs
│  │        │  └─ State/
│  │        │
│  │        ├─ Necromancer_Monster/
│  │        │  └─ State/
│  │        │
│  │        └─ Summoner_Monster/
│  │           ├─ SummonerMonster.cs
│  │           ├─ SummonerMonsterAI.cs
│  │           ├─ State/
│  │           └─ SummonedMonster/
│  │
│  ├─ Camera/
│  ├─ Utility/
│  └─ SceneMoveDoor.cs
│
├─ Animations/
├─ Materials/
├─ Prefabs/
├─ Scenes/
├─ Skill&Effect/
└─ Sprites/
```

---

## 실행 환경

- Unity `2022.3.40f1`
- PC / Windows
- C#

### 실행 방법

1. 저장소를 Clone하거나 ZIP으로 내려받습니다.
2. Unity Hub에서 프로젝트 폴더를 추가합니다.
3. Unity `2022.3.40f1` 버전으로 프로젝트를 실행합니다.
4. Build Settings에 등록된 시작 씬을 실행합니다.

Unity 버전이 다르면 일부 패키지나 프로젝트 설정이 변경될 수 있으므로 동일한 버전 사용을 권장합니다.

---

## 플레이 영상

[![YouTube](https://img.shields.io/badge/YouTube-Play_Video-red?logo=youtube)](https://www.youtube.com/watch?v=QzaXdVoEDk0)

[YouTube에서 KEMET 플레이 영상 보기](https://www.youtube.com/watch?v=QzaXdVoEDk0)
