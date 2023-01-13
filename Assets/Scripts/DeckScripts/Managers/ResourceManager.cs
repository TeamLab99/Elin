using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//오브젝트 풀링이 적용된 리소스 매니저
//데이터를 불러오고 인스턴시에트 하는 역할
public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        // original도 이미 들고 있으면 바로 사용
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index > 0)
                name = name.Substring(index + 1);

            //풀링되어 있다면
            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        //resurces.Load를 사용 중이다. 이 함수는 프로그램이 돌아가는 중 딱 한 번 정도는 사용할만하지만 여러 번 사용하는 걸 권장하지는 않는다.
        //따라서 오브젝트 풀을 만들 때 한 번에 만들어서 가지고 있는 편이 낫다.
        return Resources.Load<T>(path);
    }

    public void saveData()
    {
        //조금 더 범용적인 데이터 세이브 함수
        //아직 R&D가 조금 필요하다
        //일단은 덱 세이브에 초점을 맞춤
        List<UnlockCard> deckCardsList = new List<UnlockCard>();

        //덱에 세이브하기 위한 하나의 클래스 -> 여러 개의 데이터들을 다 담고 있는 역할
        DeckSaveData _decksave = new DeckSaveData();

        //반복문을 통해 딕셔너리에서 인스턴스들만 가져옴
        foreach (KeyValuePair<int, UnlockCard> cards in Managers.Data.DeckDict)
        {
            deckCardsList.Add(cards.Value);
        }

        //리스트를 배열로 바꿔 저장
        _decksave.deckCards = deckCardsList.ToArray();

        //인스턴스를 저장할 데이터로 변환
        string ToJsonData = JsonUtility.ToJson(_decksave, true);

        //저장되는 파일의 주소 -> Application.datPath를 이용하면 프로젝트의 Asset폴더 위치를 찾아줌
        //다른 사람의 컴퓨터 환경에서도 잘 작동함
        string filePath = Application.dataPath + "/Resources/Data/DeckData2.json";

        File.WriteAllText(filePath, ToJsonData);
    }

    //프리팹에 있는 컨텐츠를 인스턴시에트 해주는 함수
    public GameObject Instantiate(string path, Transform parent = null)
    {
        //Resources 산하의 Prefabs 폴더에서 해당 이름에 해당하는 컨텐츠를 찾는다.
        //Resources.Load를 사용해서 가능한 방법
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        //풀링이 되어 있다면 풀에서 pop해준다. Load를 하지 않고
        //이 부분의 GetComponent를 어떻게 할 수는 없을까?
        //out으로 반드시 어딘가에 넣어줘야 하는데 얘는 그럴 필요가 없어서 ...
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        //만약에 풀링이 필요하다면 -> 풀링 매니저에게 위탁
        //삭제하는 것이 아니라 오브젝트 풀로 이동시키는 것이라 생각하면 된다.
        go.TryGetComponent(out Poolable poolable);

        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);

    }
}
