using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사운드를 관리하는 사운드 매니저
//현재 사용 중이진 않다
public class SoundManager
{
    //오디오소스를 담기 위한 배열을 선언해준다. 
    AudioSource[] _audioSources = new AudioSource[(int)ESound.MaxCount];

    //오디오클립을 담기 위한 딕셔너리를 선언
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        //@Managers를 생성했던 것과 같다.
        GameObject root = GameObject.FindWithTag("Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound", tag = "Sound" };
            Object.DontDestroyOnLoad(root);

            //현재 사운드의 종류는 BGM. Effects뿐이다. MaxCount는 마지막임을 나타낼 뿐
            string[] soundNames = System.Enum.GetNames(typeof(ESound));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            //BGM이라면 무한 반복해서 재생해야하므로 loop를 true로 해준다
            _audioSources[(int)ESound.Bgm].loop = true;
        }
    }

    //오디오 소스와 클립을 싹 날려준다
    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    //재생하는 함수인데 같은 기능이지만 구현이 다른 두 가지 버젼이 존재한다.
    //하지만 내부적으로 같은 기능의 다른 함수를 호출하므로 위에 버젼을 주로 사용함
    public void Play(string path, ESound type = ESound.Effect, float pitch = 1.0f)
    {
        //넘겨받은 주소에 해당하는 오디오클립을 가져온다.
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        //오디오클립을 재생한다.
        Play(audioClip, type, pitch);
    }

    //오디오클립을 받아 재생하는 함수이다. 위의 것은 이 함수를 조금 더 편하게 사용하기 위함이다.
    public void Play(AudioClip audioClip, ESound type = ESound.Effect, float pitch = 1.0f)
    {
        //오디오클립이 없다면 종료
        if (audioClip == null)
            return;

        //종류가 BGM이라면
        if (type == ESound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)ESound.Bgm];

            //재생 중이던 BGM을 멈추고
            if (audioSource.isPlaying)
                audioSource.Stop();

            //BGM을 재생
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        //아닌 경우는 현재 Effect(효과음)밖에 없음
        else
        {
            //재생 중이던 효과음을 멈추진 않는다.
            AudioSource audioSource = _audioSources[(int)ESound.Effect];
            audioSource.pitch = pitch;
            //효과음이기에 한 번 재생
            audioSource.PlayOneShot(audioClip);
        }
    }

    //해당하는 주소에서 오디오클립을 가져오는 함수
    AudioClip GetOrAddAudioClip(string path, ESound type = ESound.Effect)
    {
        //사운드 파일 이름만 쳐도 그 파일이 Resources 산하의 Sounds라는 폴더에 있다면 찾아줄 수 있음
        //여기서도 ResourceManager의 Load함수를 사용하므로 Resources 폴더 산하에 있는 것을 기준으로 코딩됨
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == ESound.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        else
        {
            //효과음의 경우 자주 쓰일 수 있기 때문에 미리 들고 있게 한다.
            //만약 들고 있다면 Load하지 않고 들고 있지 않다면 Load하고 캐싱한다.
            //if문 안의 조건문으로 캐싱되어 있다면 audioClip에 바로 저장해준다.
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        //못 찾으면 경고문
        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");


        return audioClip;
    }
}
