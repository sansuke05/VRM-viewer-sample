using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRM;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VRMViewerSample {
	public class UIController : MonoBehaviour {

		// UI
		[SerializeField]
		private Button _importButton;

		VRMImporterContext _context;

		private void OpenVRM() {
			//ファイラーOpen
#if UNITY_EDITOR
			var path = EditorUtility.OpenFilePanel("Open VRM file", "", "vrm");
#if UNITY_EDITOR_WIN
			path = "file:///" + path;
#else
			path = "file://" + path;
#endif
#elif UNITY_STANDALONE_WIN
			var path = VRM.Samples.FileDialogForWindows.FileDialog("Open VRM file", ".vrm");
			path = "file:///" + path;
#else
			var path = Application.dataPath + "/default.vrm";
			path = "file:///" + path;
#endif
			if(path.Length != 0) {
				StartCoroutine(LoadVRMCoroutine(path));
			}
		}

		IEnumerator LoadVRMCoroutine(string path) {
			var www = new WWW(path);
			yield return www;

			// GLB形式のjson取得 -> Parse
			_context = new VRMImporterContext();
			_context.ParseGlb(www.bytes);
			modelLoad();
		}

		private void modelLoad() {
			_context.LoadAsync(_ =>
				{
					_context.ShowMeshes();
					var go = _context.Root;
					go.transform.position = new Vector3(0, 0, 1);
					go.transform.Rotate(new Vector3(0, 180, 0));
				}, Debug.LogError
			);
		}

		// Use this for initialization
		void Start () {
			_importButton.onClick.AddListener(OpenVRM);
		}

		// Update is called once per frame
		void Update () {
			
		}
	}
}