﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public List<GameObject> FollowPlayer;
    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var thing in FollowPlayer)
        {
            thing.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, thing.transform.position.z);
        }
    }
}
