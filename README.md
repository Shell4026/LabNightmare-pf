# LabNightmare pf
 코드만 있는 1인칭 공포게임 포트폴리오</br>
리소스는 외부 리소스 사용

## 구현된 대표적 기능
> 물체 상호작용 - 줍고 자유로운 회전, 던지기

https://github.com/user-attachments/assets/e02f216b-3915-4274-a243-4f58d7730992

R키를 누른채로 마우스 방향으로 회전 시킵니다. 간단한 쿼터니언 계산을 이용해 구현했습니다. </br>
잡힐 물체는 [Grabable](https://github.com/Shell4026/LabNightmare-pf/blob/main/Scripts/Entity/Grabable.cs) 컴포넌트를 부착하면 되며, 플레이어는 [Grabber](https://github.com/Shell4026/LabNightmare-pf/blob/main/Scripts/Player/Grabber.cs#L124) 컴포넌트를 이용해 상호 작용합니다.</br>
회전, 던지기 관련 코드도 Grabber 컴포넌트에 정의 돼 있습니다.

> 물체 상호작용 - 충돌 사운드

https://github.com/user-attachments/assets/65d45054-4835-4223-ab25-c5c9a2c2b49d

속도에 따라 콜라이더에 부딪히면 소리 크기와 괴물이 인식할 수 있는 소리의 범위가 달라지는 시스템입니다.</br>
[PhysSound](https://github.com/Shell4026/LabNightmare-pf/blob/main/Scripts/Entity/PhysSound.cs)컴포넌트에서 수행하며 동적으로 사운드를 생성하지 않고 오브젝트 풀에서 미리 만들어서 쓰므로 불필요한 메모리 할당이 없어 성능에 영향이 가지 않습니다. </br>
오브젝트 풀은 [PoolAble](https://github.com/Shell4026/LabNightmare-pf/blob/main/Scripts/Entity/PoolAble.cs)을 상속한 객체만 생성할 수 있습니다.

> 플레이어 조작

https://github.com/user-attachments/assets/96afae50-19a3-479b-ab21-378b21bacb6c

[PlayerController](https://github.com/Shell4026/LabNightmare-pf/blob/main/Scripts/Player/PlayerController.cs#L109)컴포넌트를 만들어 플레이어의 움직임을 구현했습니다. </br>
위 사운드 시스템과 더불어 [PlayerSound](https://github.com/Shell4026/LabNightmare-pf/blob/main/Scripts/Player/PlayerSound.cs)컴포넌트에 의해 플레이어의 속력에 따라 소리의 범위와 재생 빈도가 달라집니다. </br>
![image](https://github.com/user-attachments/assets/547932bd-c527-4bcc-aa47-789ca4492582)</br>
움직일 때 화면의 자연스러운 흔들림은 애니메이션을 이용했습니다. CamMoving 오브젝트의 움직임을 통해 걷는 듯한 움직임을 보여줍니다.

> 괴물의 AI

총 세종류가 있으며 각각의 괴물의 행동 패턴을 FSM(유한 상태 머신)을 통해 다르게 구현했습니다.</br>
모두 [Monster](https://github.com/Shell4026/LabNightmare-pf/blob/main/Scripts/Entity/Monster/Monster.cs) 클래스를 상속합니다.</br>
![기본FSM](https://github.com/user-attachments/assets/a924ab5f-5de9-405c-8ab7-788376062d3f)</br>
- 기본 몬스터 상태
1. [Scout](https://github.com/Shell4026/LabNightmare-pf/blob/main/Scripts/Entity/Monster/Scout.cs) - 괴물의 머리를 쳐다보면 안 됩니다. Raycast를 통해 머리를 쳐다보면 Hunt 상태가 됩니다.
2. [Tank](https://github.com/Shell4026/LabNightmare-pf/blob/main/Scripts/Entity/Monster/Tank.cs) - 시각이 없으나 청각에 예민합니다. 위의 충돌 사운드 시스템과 연계 됩니다. aggro 수치가 꽉차면 Hunt 상태가 됩니다.
3. [Charger](https://github.com/Shell4026/LabNightmare-pf/blob/main/Scripts/Entity/Monster/Charger.cs) - 시야에서 벗어나면 Hunt 상태가 되며, 쳐다보면 멈추고 플레이어를 처음 발견하기 전까지는 맵을 돌아다니며 순찰만 합니다.
