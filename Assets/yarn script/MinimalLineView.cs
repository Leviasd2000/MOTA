using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Yarn.Unity;

public class MinimalLineView : MonoBehaviour
{
    [SerializeField] internal CanvasGroup canvasGroup;

    [SerializeField] internal bool useFadeEffect = true;

    [SerializeField]
    [Min(0)]
    internal float fadeInTime = 0.25f;

    [SerializeField]
    [Min(0)]
    internal float fadeOutTime = 0.05f;

    [SerializeField] internal TextMeshProUGUI lineText = null;

    [SerializeField] internal bool showCharacterNameInLineView = true;

    [SerializeField] internal TextMeshProUGUI characterNameText = null;

    [SerializeField] internal bool useTypewriterEffect = false;

    [SerializeField]
    [Min(0)]
    internal float typewriterEffectSpeed = 0f;

    [SerializeField] internal GameObject continueButton = null;

    [SerializeField]
    [Min(0)]
    internal float holdTime = 1f;

    LocalizedLine currentLine = null;

    Effects.CoroutineInterruptToken currentStopToken = new Effects.CoroutineInterruptToken();

    private MinimalDialogueRunner runner;

    private void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
    void Start() { runner = FindFirstObjectByType<MinimalDialogueRunner>(); }
    private void Reset() { canvasGroup = GetComponentInParent<CanvasGroup>(); }

    public void DismissLine()
    {
        currentLine = null;

        StartCoroutine(DismissLineInternal());
    }

    private IEnumerator DismissLineInternal()
    {
        var interactable = canvasGroup.interactable;
        canvasGroup.interactable = false;

        if (useFadeEffect)
        {
            yield return StartCoroutine(Effects.FadeAlpha(canvasGroup, 1, 0, fadeOutTime, currentStopToken));
            currentStopToken.Complete();
        }

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = interactable;
        runner.Continue();
    }

    public void RunLine(LocalizedLine dialogueLine)
    {
        StopAllCoroutines();
        StartCoroutine(RunLineInternal(dialogueLine));
    }

    private IEnumerator RunLineInternal(LocalizedLine dialogueLine)
    {
        IEnumerator PresentLine()
        {
            lineText.gameObject.SetActive(true);
            canvasGroup.gameObject.SetActive(true);

            if (continueButton != null)
            {
                continueButton.SetActive(false);
            }

            if (characterNameText != null)
            {
                characterNameText.text = dialogueLine.CharacterName;
                lineText.text = dialogueLine.TextWithoutCharacterName.Text;
            }
            else
            {
                if (showCharacterNameInLineView)
                {
                    lineText.text = dialogueLine.Text.Text;
                }
                else
                {
                    lineText.text = dialogueLine.TextWithoutCharacterName.Text;
                }
            }

            if (useTypewriterEffect)
            {
                lineText.maxVisibleCharacters = 0;
            }
            else
            {
                lineText.maxVisibleCharacters = int.MaxValue;
            }

            if (useFadeEffect)
            {
                yield return StartCoroutine(Effects.FadeAlpha(canvasGroup, 0, 1, fadeInTime, currentStopToken));
                if (currentStopToken.WasInterrupted)
                {
                    yield break;
                }
            }

            if (useTypewriterEffect)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                yield return StartCoroutine(
                    Effects.Typewriter(
                        lineText,
                        typewriterEffectSpeed,
                        null,
                        currentStopToken
                    )
                );
                if (currentStopToken.WasInterrupted)
                {
                    yield break;
                }
            }
        }
        currentLine = dialogueLine;

        yield return StartCoroutine(PresentLine());

        currentStopToken.Complete();
        lineText.maxVisibleCharacters = int.MaxValue;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (holdTime > 0)
        {
            yield return new WaitForSeconds(holdTime);
        }
        if (continueButton != null)
        {
            continueButton.SetActive(true);
        }
    }

    public void OnContinueClicked()
    {
        if (currentLine == null)
        {
            return;
        }
        DismissLine();
    }
}