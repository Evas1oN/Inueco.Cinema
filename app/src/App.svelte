<script>
  import Cinema from "./lib/Cinema.svelte";
  import Films from "./lib/Films.svelte";
  import Genres from "./lib/Genres.svelte";
  import Auth from "./lib/Auth.svelte";
  import { user } from "./stores/user";
  import { get } from "svelte/store";
  import { onMount } from "svelte";

  let auth = false;

  user.subscribe(value => {
    if (value != undefined) {
      auth = true;
      return;
    }
    auth = false;
  })

  onMount(async () => {
    let response = await fetch("/api/auth")
    console.log(get(user))
    if (response.ok) {
      user.set(await response.json());
      return;
    }
    user.set(undefined)
  })

  let selectedComponent = null
  let components = [
    {
      instance: Cinema,
      name: "Сеансы"
    },
    {
      instance: Genres,
      name: "Жанры"
    },
    {
      instance: Films,
      name: "Фильмы"
    }
  ]
</script>

{#if !auth}
  <div class="container">
    <Auth />
  </div>
{/if}


<nav class="container">
  <ul>
    <li>
      <strong>Inueco</strong>
      {#if auth}
        ({get(user).username})
      {/if}
    </li>
  </ul>

  {#if auth}
    <ul>
      {#each components as component}
        {#if selectedComponent == component.instance}
          <strong><li><button style="all:unset; cursor: pointer" on:click={() => selectedComponent = component.instance}>{component.name}</button></li></strong>
        {:else}
          <li><button style="all:unset; cursor: pointer" on:click={() => selectedComponent = component.instance}>{component.name}</button></li>
        {/if}
      {/each}
    </ul>
  {/if}
</nav>

<div class="container">
  <svelte:component this={selectedComponent}></svelte:component>
</div>
