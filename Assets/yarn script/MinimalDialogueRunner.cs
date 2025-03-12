using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.Events;
using System.Linq;

public class MinimalDialogueRunner : MonoBehaviour
{
    public YarnProject project;
    public VariableStorageBehaviour VariableStorage;
    public LineProviderBehaviour LineProvider;

    public bool isRunning { get; internal set; } = false;

    private Yarn.Dialogue dialogue;
    private void PrepareForLines(IEnumerable<string> lineIDs) { LineProvider.PrepareForLines(lineIDs); }
    public bool NodeExists(string nodeName) => dialogue.NodeExists(nodeName);
    public UnityEvent<DialogueOption[]> OptionsNeedPresentation;
    public UnityEvent<string[]> CommandNeedsHandling;
    public UnityEvent<LocalizedLine> LineNeedsPresentation;
    public UnityEvent<string> NodeStarted;
    public UnityEvent<string> NodeEnded;
    public UnityEvent DialogueComplete;

    public void StartDialogue(string nodeName = "Start")
    {
        if (isRunning)
        {
            Debug.LogWarning("Can't start a dialogue that is already running");
            return;
        }
        isRunning = true;
        dialogue.SetNode(nodeName);
        dialogue.Continue();
    }
    public void StopDialogue()
    {
        dialogue.Stop();
        isRunning = false;
    }

    private void HandleOptions(Yarn.OptionSet options)
    {
        DialogueOption[] optionSet = new DialogueOption[options.Options.Length];
        for (int i = 0; i < options.Options.Length; i++)
        {
            var line = LineProvider.GetLocalizedLine(options.Options[i].Line);
            var text = Yarn.Dialogue.ExpandSubstitutions(line.RawText, options.Options[i].Line.Substitutions);
            // dialogue.LanguageCode = LineProvider.textLanguageCode;
            line.Text = dialogue.ParseMarkup(text);

            optionSet[i] = new DialogueOption
            {
                TextID = options.Options[i].Line.ID,
                DialogueOptionID = options.Options[i].ID,
                Line = line,
                IsAvailable = options.Options[i].IsAvailable,
            };
        }
        OptionsNeedPresentation?.Invoke(optionSet);
    }

    private void HandleCommand(Yarn.Command command)
    {
        var elements = Yarn.Unity.DialogueRunner.SplitCommandText(command.Text).ToArray();

        if (elements[0] == "wait")
        {
            if (elements.Length < 2)
            {
                Debug.LogWarning("Asked to wait but given no duration!");
                return;
            }
            float duration = float.Parse(elements[1]);
            if (duration > 0)
            {
                IEnumerator Wait(float time)
                {
                    isRunning = false;
                    yield return new WaitForSeconds(time);
                    isRunning = true;
                    Continue();
                }
                StartCoroutine(Wait(duration));
            }
        }
        else
        {
            CommandNeedsHandling?.Invoke(elements);
        }
    }
    private void HandleLine(Yarn.Line line)
    {
        var finalLine = LineProvider.GetLocalizedLine(line);
        var text = Yarn.Dialogue.ExpandSubstitutions(finalLine.RawText, line.Substitutions);
        // dialogue.LanguageCode = LineProvider.textLanguageCode;
        finalLine.Text = dialogue.ParseMarkup(text);

        LineNeedsPresentation?.Invoke(finalLine);
    }
    private void HandleNodeStarted(string nodeName)
    {
        NodeStarted?.Invoke(nodeName);
    }
    private void HandleNodeEnded(string nodeName)
    {
        NodeEnded?.Invoke(nodeName);
    }
    private void HandleDialogueComplete()
    {
        isRunning = false;
        DialogueComplete?.Invoke();
    }
    public void Continue()
    {
        if (!isRunning)
        {
            Debug.LogWarning("Can't continue dialogue when we aren't currently running any");
            return;
        }

        dialogue.Continue();
    }
    public void SetSelectedOption(int optionIndex)
    {
        if (!isRunning)
        {
            Debug.LogWarning("Can't select an option when not currently running dialogue");
            return;
        }
        dialogue.SetSelectedOption(optionIndex);
        dialogue.Continue();
    }
    void Awake()
    {
        if (VariableStorage == null)
        {
            VariableStorage = gameObject.AddComponent<InMemoryVariableStorage>();
        }
        dialogue = CreateDialogueInstance();
        /// dialogue.SetProgram(project.GetProgram());

        if (LineProvider == null)
        {
            LineProvider = gameObject.AddComponent<TextLineProvider>();
        }
        LineProvider.YarnProject = project;
    }
    private Yarn.Dialogue CreateDialogueInstance()
    {
        var dialogue = new Yarn.Dialogue(VariableStorage)
        {
            LogDebugMessage = delegate (string message)
            {
                Debug.Log(message);
            },
            LogErrorMessage = delegate (string message)
            {
                Debug.LogError(message);
            },

            LineHandler = HandleLine,
            CommandHandler = HandleCommand,
            OptionsHandler = HandleOptions,
            NodeStartHandler = HandleNodeStarted,
            NodeCompleteHandler = HandleNodeEnded,
            DialogueCompleteHandler = HandleDialogueComplete,
            PrepareForLinesHandler = PrepareForLines
        };
        return dialogue;
    }


}

