﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Build : MonoBehaviour
{
	public Room roomBlueprint;
    public Room currentRoom;

	public bool blueprinting = false;
	
	public RoomManager roomManager;
    public InventoryManager inventoryManager;
    public Player player;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;
    public MusicManager musicManager;

    bool moving = false;

	// Start is called before the first frame update
	private void Start()
	{
		roomManager = FindObjectOfType<RoomManager>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        player = FindObjectOfType<Player>();
        musicManager = FindObjectOfType<MusicManager>();

        //roomBlueprintObject = Instantiate(roomObjects[currentRoom], transform);
        //roomBlueprint = roomBlueprintObject.GetComponent<Room>();
        //roomBlueprint.isBlueprint = true;

        //roomBlueprint.Hidden(true);		
    }

	// Update is called once per frame
	private void Update()
	{
        if (blueprinting)
		{
            var xDir = Input.GetAxis("Horizontal");
            var yDir = Input.GetAxis("Vertical");

            if(moving && (xDir == 0 && yDir == 0) && !Input.GetButton("Inventory Use"))
            {
                moving = false;
            }

            if(!moving && (xDir != 0 || yDir != 0))
            {
                moving = true;
                int moveX = 0;
                int moveY = 0;

                if (xDir > 0) moveX = 1;
                else if (xDir < 0) moveX = -1;
                else if (yDir < 0) moveY = -1;
                else if (yDir > 0) moveY = 1;

                roomBlueprint.transform.position += new Vector3(moveX, moveY);

            }

            if (!roomManager.CanPlaceRoom(roomBlueprint, roomBlueprint.transform.position))
			{
				roomBlueprint.SetColor(Color.red);
			}
			else
			{
				roomBlueprint.SetColor(Color.green);

				if (!moving && Input.GetButtonDown("Inventory Use"))
				{
					BuildRoom();
				}
			}
		}
	}

	public void BeginBuild(Room room)
	{
        musicManager.SetTrackVolume(TrackType.Drums, 1, 1);
        moving = true;
        inventoryManager.InventoryDisabled = true;
        player.Building = true;        
        currentRoom = room;
        if(roomBlueprint != null)
        {
            Destroy(roomBlueprint.gameObject);
        }
		
		roomBlueprint = Instantiate(currentRoom, new Vector3(player.transform.position.x, player.transform.position.y + 1), transform.rotation);
		roomBlueprint.isBlueprint = true;
        blueprinting = true;
        virtualCamera.Follow = roomBlueprint.transform;
        virtualCamera.enabled = true;
    }		

	private Vector2 GetCentreOfShape(List<Vector2> Shape)
	{
		var xMax = Shape.OrderByDescending(x => x.x).First().x + 1;
		var yMax = Shape.OrderByDescending(x => x.y).First().y + 1;

		return new Vector2(xMax / 2, yMax / 2);
	}

	private Vector2 SnapToGrid(Vector2 position)
	{
		return new Vector2((float)Math.Round(position.x, 0), (float)Math.Round(position.y, 0));
	}

	private void BuildRoom()
	{
        Instantiate(currentRoom, roomBlueprint.transform.position, transform.rotation);
        Destroy(roomBlueprint.gameObject);
        roomBlueprint = null;
        blueprinting = false;
        inventoryManager.InventoryDisabled = false;
        player.Building = false;
        musicManager.SetTrackVolume(TrackType.Drums, 0, 10);
        virtualCamera.enabled = false;
    }
}