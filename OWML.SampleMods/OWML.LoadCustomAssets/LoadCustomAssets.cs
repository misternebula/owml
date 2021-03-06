﻿using OWML.Common;
using OWML.ModHelper;
using UnityEngine;

namespace OWML.LoadCustomAssets
{
    public class LoadCustomAssets : ModBehaviour
    {
        private OWRigidbody _duckBody;
        private Transform _playerTransform;
        private OWRigidbody _playerBody;
        private AudioSource _shootSound;
        private AudioSource _music;
        private SaveFile _saveFile;
        private GameObject _cube;

        private bool _isStarted;
        private bool _isDucksEnabled;
        private bool _isCubesEnabled;

        private void Start()
        {
            ModHelper.Console.WriteLine($"In {nameof(LoadCustomAssets)}!");
            _saveFile = ModHelper.Storage.Load<SaveFile>("savefile.json");
            ModHelper.Console.WriteLine("Ducks shot: " + _saveFile.NumberOfDucks);

            var assetBundle = ModHelper.Assets.LoadBundle("cubebundle");
            _cube = assetBundle.LoadAsset<GameObject>("Cube");

            var gunSoundAsset = ModHelper.Assets.LoadAudio("blaster-firing.wav");
            gunSoundAsset.OnLoaded += OnGunSoundLoaded;
            var duckAsset = ModHelper.Assets.Load3DObject("duck.obj", "duck.png");
            duckAsset.OnLoaded += OnDuckLoaded;
            var musicAsset = ModHelper.Assets.LoadAudio("spiral-mountain.mp3");
            musicAsset.OnLoaded += OnMusicLoaded;

            ModHelper.Events.Subscribe<PlayerBody>(Events.AfterAwake);
            ModHelper.Events.OnEvent += OnEvent;

            var modMenu = ModHelper.Menus.ModsMenu.GetModMenu(this);
        }

        public override void Configure(IModConfig config)
        {
            ToggleMusic(config.GetSettingsValue<bool>("enableMusic"));
            _isDucksEnabled = config.GetSettingsValue<bool>("enableDucks");
            _isCubesEnabled = config.GetSettingsValue<bool>("enableCubes");
            var speed = config.GetSettingsValue<float>("speed");
            var power = config.GetSettingsValue<float>("power");
            var enableSuperMode = config.GetSettingsValue<bool>("enableSuperMode");
        }

        private void OnMusicLoaded(AudioSource audio)
        {
            _music = audio;
            ModHelper.Console.WriteLine("Music loaded!");
        }

        private void OnGunSoundLoaded(AudioSource audio)
        {
            _shootSound = audio;
            ModHelper.Console.WriteLine("Gun sound loaded!");
        }

        private void OnDuckLoaded(GameObject duck)
        {
            ModHelper.Console.WriteLine("Duck loaded!");
            duck.AddComponent<SphereCollider>();
            duck.AddComponent<Rigidbody>();
            _duckBody = duck.AddComponent<OWRigidbody>();
        }

        private void OnEvent(MonoBehaviour behaviour, Events ev)
        {
            if (behaviour.GetType() == typeof(PlayerBody) && ev == Events.AfterAwake)
            {
                _playerBody = (PlayerBody)behaviour;
                _playerTransform = behaviour.transform;
                _isStarted = true;
                ToggleMusic(ModHelper.Config.GetSettingsValue<bool>("enableMusic"));
            }
        }

        private void Update()
        {
            if (!_isStarted || OWTime.IsPaused())
            {
                return;
            }
            if (Input.GetMouseButtonDown(0) && _isDucksEnabled)
            {
                ShootDuck();
            }
            else if (Input.GetMouseButtonDown(1) && _isCubesEnabled)
            {
                CreateCube();
            }
        }

        private void ShootDuck()
        {
            var duckBody = Instantiate(_duckBody);

            duckBody.SetPosition(_playerTransform.position + _playerTransform.forward * 2f);
            duckBody.SetRotation(_playerTransform.rotation);
            duckBody.SetVelocity(_playerBody.GetVelocity() + _playerTransform.forward * 10f);
            _shootSound.Play();

            _saveFile.NumberOfDucks++;
            ModHelper.Console.WriteLine("Ducks shot:", _saveFile.NumberOfDucks);
            ModHelper.Storage.Save(_saveFile, "savefile.json");
        }

        private void CreateCube()
        {
            Instantiate(_cube, _playerTransform.position + _playerTransform.forward * 2f, Quaternion.identity);
        }

        private void ToggleMusic(bool enable)
        {
            ModHelper.Console.WriteLine("ToggleMusic:", enable);
            if (_music == null)
            {
                return;
            }
            if (enable && _isStarted)
            {
                _music.Play();
            }
            if (!enable)
            {
                _music.Stop();
            }
        }

    }
}
