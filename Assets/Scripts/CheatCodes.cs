using System.Collections.Generic;
using Avocado.DeveloperCheatConsole.Scripts.Core;
using Avocado.DeveloperCheatConsole.Scripts.Core.Commands;
using UnityEngine;

namespace Avocado.DeveloperCheatConsole.Scripts.Core.Commands.AllCommands {
    public class CheatCodes : MonoBehaviour {
        private void Awake() {
            DeveloperConsole.Instance.AddCommand(new DevCommand("add_money", "add_money [amount]", delegate(int parameter) {
                DataManager.AddMoney(parameter);
            }));

            DeveloperConsole.Instance.AddCommand(new DevCommand("add_exp", "add_exp [points]", delegate(int parameter) {
                DataManager.AddExp(parameter);
            }));
            
            DeveloperConsole.Instance.AddCommand(new DevCommand("unlock_course", "unlock_course [course_name]", delegate(string parameter) {
                if (DataManager.GetProgrammingLanguageProgress(parameter) <= 0) DataManager.AddProgrammingLanguageProgress(parameter);
            }));
            
            DeveloperConsole.Instance.AddCommand(new DevCommand("complete_courses", "This will complete all the available courses", () => {
                DataManager.CompleteProgrammingLanguages();
            }));
            
            DeveloperConsole.Instance.AddCommand(new DevCommand("complete_course", "complete_course [course_name]", delegate(string parameter) {
                DataManager.SetProgrammingLanguageProgress(parameter, 12);
            }));

            DeveloperConsole.Instance.AddCommand(new DevCommand("test_course", "test_course [course_name]", delegate(string parameter) {
                DataManager.SetProgrammingLanguageProgress(parameter, 10);
            }));

            DeveloperConsole.Instance.AddCommand(new DevCommand("unlock_interactions", "This will unlock all the available interactions", () => {
                InteractionQuizManager.Instance.ActivateAllInteractionQuiz();
            }));
            
            // DeveloperConsole.Instance.AddCommand(new DevCommand("test", "test with range string parameters", delegate(List<string> parameters) {
            //     Debug.Log("success execute command test with range string parameters " + string.Join(" ", parameters));
            // }));
            
            // //Add command with range number parameters
            // DeveloperConsole.Instance.AddCommand(new DevCommand("test", "test with range string parameters", delegate(List<int> parameters) {
            //     Debug.Log("success execute command test with range number parameters " + string.Join(" ", parameters));
            // }));
        }
    }
}