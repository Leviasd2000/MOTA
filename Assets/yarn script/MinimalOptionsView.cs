using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;

public class MinimalOptionsView : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] OptionView optionViewPrefab;

    [SerializeField] TextMeshProUGUI lastLineText;

    [SerializeField] float fadeTime = 0.1f;

    [SerializeField] bool showUnavailableOptions = false;

    List<OptionView> optionViews = new List<OptionView>();

    LocalizedLine lastSeenLine;

    private MinimalDialogueRunner runner;

    public void Start()
    {
        runner = FindFirstObjectByType<MinimalDialogueRunner>();

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void Reset() { canvasGroup = GetComponentInParent<CanvasGroup>(); }

    public void RunLine(LocalizedLine dialogueLine) { lastSeenLine = dialogueLine; }

    public void RunOptions(DialogueOption[] options)
    {
        System.Action<Yarn.Unity.DialogueOption> onOptionSelected = delegate (DialogueOption selectedOption)
        {
            StartCoroutine(OptionViewWasSelectedInternal(selectedOption));
            IEnumerator OptionViewWasSelectedInternal(DialogueOption selectedOption)
            {
                yield return StartCoroutine(Yarn.Unity.Effects.FadeAlpha(canvasGroup, 1, 0, fadeTime));
                runner.SetSelectedOption(selectedOption.DialogueOptionID);
            }
        };

        foreach (var optionView in optionViews) { optionView.gameObject.SetActive(false); }

        while (options.Length > optionViews.Count)
        {
            var optionView = CreateNewOptionView(onOptionSelected);
            optionView.gameObject.SetActive(false);
        }

        int optionViewsCreated = 0;

        for (int i = 0; i < options.Length; i++)
        {
            var optionView = optionViews[i];
            var option = options[i];

            if (option.IsAvailable == false && showUnavailableOptions == false) { continue; }

            optionView.gameObject.SetActive(true);

            optionView.Option = option;
            optionView.OnOptionSelected = onOptionSelected;

            if (optionViewsCreated == 0)
            {
                optionView.Select();
            }

            optionViewsCreated += 1;
        }

        if (lastLineText != null)
        {
            if (lastSeenLine != null)
            {
                lastLineText.gameObject.SetActive(true);
                lastLineText.text = lastSeenLine.Text.Text;
            }
            else
            {
                lastLineText.gameObject.SetActive(false);
            }
        }

        StartCoroutine(Effects.FadeAlpha(canvasGroup, 0, 1, fadeTime));

        OptionView CreateNewOptionView(System.Action<Yarn.Unity.DialogueOption> callback)
        {
            var optionView = Instantiate(optionViewPrefab);
            optionView.transform.SetParent(transform, false);
            optionView.transform.SetAsLastSibling();

            optionView.OnOptionSelected = callback;
            optionViews.Add(optionView);

            return optionView;
        }
    }
}
