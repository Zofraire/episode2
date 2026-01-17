using System.Collections;
using UnityEngine;

public class Lvl2 : MonoBehaviour
{
    [Header("Glow Objects")]
    [SerializeField] private GameObject redGlow;
    [SerializeField] private GameObject blueGlow;
    [SerializeField] private GameObject greenGlow;
    [SerializeField] private GameObject yellowGlow;

    [Header("Correct / Incorrect")]
    [SerializeField] private GameObject correctButton;
    [SerializeField] private GameObject incorrectButton;

    [Header("Emotions")]
    [SerializeField] private GameObject Emotions;

    [Header("Settings")]
    [SerializeField] private float glowDuration = 0.4f;
    [SerializeField] private float signDuration = 3.0f;
    [SerializeField] private float delayBetweenGlows = 0.3f;
    [SerializeField] private float emotionDuration = 12.0f;

    [Header("Text Objects")]
    [SerializeField] private GameObject text_1;
    [SerializeField] private GameObject text_2;
    [SerializeField] private GameObject text_3;
    [SerializeField] private GameObject text_4;
    [SerializeField] private GameObject text_5;
    [SerializeField] private GameObject text_6;

    [SerializeField] private Animator emotionsAnimator;
    [SerializeField] private Animator centerAnimator;
    [SerializeField] private Animator curtainsAnimator;
    [SerializeField] private GameObject endWindow;

    public AudioSource round1Source;
    public AudioClip round1Clip;

    public AudioSource round2Source;
    public AudioClip round2Clip;

    public AudioSource round3Source;
    public AudioClip round3Clip;

    public AudioSource round4Source;
    public AudioClip round4Clip;

    public AudioSource round5Source;
    public AudioClip round5Clip;

    public AudioSource round6Source;
    public AudioClip round6Clip;

    public AudioSource wrongSource;
    public AudioClip wrongClip;

    public AudioSource attentionSource;
    public AudioClip attentionClip;

    public AudioSource congratulationsSource;
    public AudioClip congratulationsClip;

    public AudioSource endSource;
    public AudioClip endClip;

    private int[] correctSequence;
    private int currentIndex;
    private int round = 1;
    private const int maxRounds = 6;

    private bool playerTurn = false;

    public void StartRound()
    {
        currentIndex = 0;
        GenerateSequence(round);
        StartCoroutine(PlaySequence());
        PlayerPrefs.SetInt("levelsUnlocked", 3);
        PlayerPrefs.Save();
    }

    void GenerateSequence(int length)
    {
        correctSequence = new int[length];

        for (int i = 0; i < length; i++)
        {
            correctSequence[i] = Random.Range(0, 4);
        }
    }

    IEnumerator PlaySequence()
    {
        playerTurn = false;
        attentionSource.PlayOneShot(attentionClip);

        yield return new WaitForSeconds(1.0f);

        foreach (int color in correctSequence)
        {
            yield return StartCoroutine(GlowColor(color));
            yield return new WaitForSeconds(delayBetweenGlows);
        }

        playerTurn = true;
    }

    // ===================== PLAYER INPUT =====================
    public void PressRed() { CheckInput(0); }
    public void PressBlue() { CheckInput(1); }
    public void PressGreen() { CheckInput(2); }
    public void PressYellow() { CheckInput(3); }

    void CheckInput(int input)
    {
        if (!playerTurn) return;

        StartCoroutine(GlowColor(input));

        if (correctSequence[currentIndex] != input)
        {
            StartCoroutine(Incorrect());
            return;
        }

        currentIndex++;

        if (currentIndex >= correctSequence.Length)
        {
            StartCoroutine(Correct());
        }
    }

    // ===================== GAME STATES =====================
    IEnumerator Correct()
    {
        playerTurn = false;

        centerAnimator.SetTrigger("correct");
        congratulationsSource.PlayOneShot(congratulationsClip);
        yield return new WaitForSeconds(1f);

        Emotions.SetActive(true);
        switch (round)
        {
            case 1:
                
                round1Source.PlayOneShot(round1Clip);
                break;
            case 2:
                round2Source.PlayOneShot(round2Clip);

                break;
            case 3:
                round3Source.PlayOneShot(round3Clip);
                break;
            case 4:
                round4Source.PlayOneShot(round4Clip);
                break;
            case 5:
                round5Source.PlayOneShot(round5Clip);
                break;
            case 6:
                round6Source.PlayOneShot(round6Clip);
                break;
            default:
                break;
        }

        string triggerName = $"round_{round}";
        emotionsAnimator.SetTrigger(triggerName);

        if (round <= maxRounds)
        {
            switch (round)
            {
                case 1:
                    text_1.SetActive(true);
                    yield return new WaitForSeconds(7.0f);
                    text_1.SetActive(false);
                    break;
                case 2:
                    text_2.SetActive(true);
                    yield return new WaitForSeconds(10.0f);
                    text_2.SetActive(false);
                    break;
                case 3:
                    text_3.SetActive(true);
                    yield return new WaitForSeconds(11.0f);
                    text_3.SetActive(false);
                    break;
                case 4:
                    text_4.SetActive(true);
                    yield return new WaitForSeconds(8.0f);
                    text_4.SetActive(false);
                    break;
                case 5:
                    text_5.SetActive(true);
                    yield return new WaitForSeconds(10.0f);
                    text_5.SetActive(false);
                    break;
                case 6:
                    text_6.SetActive(true);
                    yield return new WaitForSeconds(9.0f);
                    text_6.SetActive(false);
                    break;
                default:
                    break;
            }
            Emotions.SetActive(false);
            correctButton.SetActive(false);

            round++;

            StartRound();
        }

        else if (round > maxRounds)
        {
            endSource.PlayOneShot(endClip);
            curtainsAnimator.SetTrigger("CurtainsClose");
            endWindow.SetActive(true);
            yield break;
        }
        
    }


    IEnumerator Incorrect()
    {
        playerTurn = false;

        centerAnimator.SetTrigger("incorrect");
        wrongSource.PlayOneShot(wrongClip);

        currentIndex = 0; 

        yield return new WaitForSeconds(3f);

        StartCoroutine(PlaySequence());
    }

    // ===================== GLOW =====================
    IEnumerator GlowColor(int color)
    {
        GameObject glow = color switch
        {
            0 => redGlow,
            1 => blueGlow,
            2 => greenGlow,
            3 => yellowGlow,
            _ => null
        };

        glow.SetActive(true);
        yield return new WaitForSeconds(glowDuration);
        glow.SetActive(false);
    }
}
