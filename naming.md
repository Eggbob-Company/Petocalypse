# 네이밍 컨벤션
### - 변수 : snake case 사용
> ex) base_damage, base_hp 등

### - bool 변수 : 변수 앞에 is 추가
> ex) is_attack, is_move 등

### - private 변수 : 변수 앞에 _ 추가
> ex) _is_attack, _is_move 등

### - 함수 : pascal case 사용
> ex) PlayerAttack(), PlayerMove() 등

<br>

# 개발 주의사항
### 1. 개발 시작하기 전 꼭 브랜치 바꾸고 시작하기
> git checkout -b 브랜치명
### 2. 커밋하기 전에 유니티 프로젝트 저장 후 종료한 다음 커밋하고 푸쉬하기
> git commit -m “커밋 내용”
> git push origin 브랜치명
### 3. 씬에 요소 배치할 때 꼭 본인 테스트 씬인지 확인할 것!!”

<br>

# 브랜치 변경 주의사항
### 브랜치를 새로 만들 때에는 꼭 develop 브랜치의 최신 버전 상태에서 만들기
> git checkout develop  
> git pull origin develop  
> git checkout -b 브랜치명
### 브랜치를 새로 만들 때에는 꼭 develop 브랜치에서 만들기  
### -> '저번에 pull한 이후로 merge된 거 없으니까~' 하는 안일한 생각으로 feat 브랜치에서 만들지 말기
> git checkout develop
> git checkout -b 브랜치명
