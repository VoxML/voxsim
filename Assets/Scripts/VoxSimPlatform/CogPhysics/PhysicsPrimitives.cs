﻿using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;

using VoxSimPlatform.Core;
using VoxSimPlatform.Global;
using VoxSimPlatform.Vox;

namespace VoxSimPlatform {
    namespace CogPhysics {
        public class PhysicsPrimitives : MonoBehaviour {
        	bool resolveDiscrepancies;
        	EventManager eventManager;

        	const double PHYSICS_CATCHUP_TIME = 100.0;
        	Timer catchupTimer;

        	EventManagerArgs testSatisfied;

        	// Use this for initialization
        	void Start() {
        		eventManager = GameObject.Find("BehaviorController").GetComponent<EventManager>();

        		resolveDiscrepancies = false;

        		catchupTimer = new Timer(PHYSICS_CATCHUP_TIME);
        		catchupTimer.Enabled = false;
        		catchupTimer.Elapsed += Resolve;

        		eventManager.EventComplete += EventSatisfied;
        	}

        	// Update is called once per frame
        	void Update() {
        	}

        	void LateUpdate() {
        		//if (Input.GetKeyDown (KeyCode.R)) {
        		if (resolveDiscrepancies) {
        			//Debug.Log ("resolving");
        			PhysicsHelper.ResolveAllPhysicsDiscrepancies(testSatisfied.MacroEvent);
        			//Debug.Break ();
        			if (eventManager.events.Count > 0) {
        				catchupTimer.Interval = 1;
        			}

        			Hashtable predArgs = Helper.ParsePredicate(testSatisfied.EventString);
        			String predString = "";
        			String[] argsStrings = null;

        			foreach (DictionaryEntry entry in predArgs) {
        				predString = (String) entry.Key;
        				argsStrings = ((String) entry.Value).Split(new char[] {','});
        			}

                    // if any object in argsStrings VoxML contains affordances with event predString
                    //  reason the consequences of those affordances
                    foreach (String argString in argsStrings) {
                        // find the GameObject of this name
                        GameObject argObj = GameObject.Find(argString);
                        if (argObj != null) {
                            // get its Voxeme component
                            Voxeme argVox = argObj.GetComponent<Voxeme>();
                            if (argVox != null) {
                                // find all affordances in argVox.voxml.Afford_Str (that is, in the object's affordance structure)
                                //  that contain predString in the event E (not the result R) -- viz. affordance encoding format H->[E]R
                                // Regex matches [predString(...)]
                                Regex r = new Regex("\\["+predString+"\\(.+\\)\\]");
                                // if there's >0 affordances in argObj's affordance structure that match predString
                                // reason the consequences of those affordances
                                if (argVox.voxml.Afford_Str.Affordances.Where(a => r.IsMatch(a.Formula)).ToList().Count > 0) {
                                    SatisfactionTest.ReasonFromAffordances(eventManager, testSatisfied.VoxML, predString, argVox);
                                }
                            }
                        }
                    }

        			// TODO: better than this
        			// which predicates result in affordance-based consequence?
        			if ((predString == "ungrasp") || (predString == "lift") ||
        			    (predString == "turn") || (predString == "roll") ||
        			    (predString == "slide") || (predString == "put")) {
        				SatisfactionTest.ReasonFromAffordances(eventManager, null, predString,
        					GameObject.Find(argsStrings[0] as String)
        						.GetComponent<Voxeme>()); // we need to talk (do physics reactivation in here?) // replace ReevaluateRelationships
        			}
        		}

        		//}
        	}

        	void EventSatisfied(object sender, EventArgs e) {
                testSatisfied = (EventManagerArgs)e;
        		resolveDiscrepancies = true;
        		catchupTimer.Enabled = true;

                Debug.Log(string.Format("Satisfaction condition met for {0} specification {1}",
                    testSatisfied.VoxML.Lex.Pred,testSatisfied.EventString));
        	}

        	void Resolve(object sender, ElapsedEventArgs e) {
        		catchupTimer.Enabled = false;
        		catchupTimer.Interval = PHYSICS_CATCHUP_TIME;
        		resolveDiscrepancies = false;
        	}
        }
    }
}